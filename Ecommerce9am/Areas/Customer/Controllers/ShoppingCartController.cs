using Ecommerce9am.Data.Repository;
using Ecommerce9am.Data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Stripe.Checkout;
using System.Security.Claims;
using Utils;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;

namespace Ecommerce9am.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = StaticData.ROLE_CUSTOMER)]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        private ApplicationUser user { get; set; }
        [BindProperty]
        private ShoppingCartViewModel CartVMObj { get; set; }
        public ShoppingCartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        [HttpGet]
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            String userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartViewModel cartObj = new()
            {
                shoppingCarts = _unitOfWork.shoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader=new()
            };
            foreach(var item in cartObj.shoppingCarts)
            {
                cartObj.OrderHeader.OrderTotal += item.Quantity * item.Product.Price;
            }
            return View(cartObj);
        }
        [HttpPost]
        public IActionResult AddToCart(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            String userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cartFromDb = _unitOfWork.shoppingCart.FirstOrDefault(u => u.ProductId == shoppingCart.ProductId);

            if (cartFromDb == null)
            {
                _unitOfWork.shoppingCart.Create(shoppingCart);
                _unitOfWork.Save();
                TempData["success"] = "Item added to cart.";
            }
            else
            {
                cartFromDb.Quantity += shoppingCart.Quantity;
                _unitOfWork.Save();
                TempData["success"] = "Item quantity updated in cart.";
            }
            return RedirectToAction("Index");
        }
        public IActionResult Increase(int ProductId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            String userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCart cartObj = _unitOfWork.shoppingCart.FirstOrDefault(u => u.ProductId == ProductId && u.ApplicationUserId == userId);

            if (cartObj != null)
            {
                cartObj.Quantity++;
                _unitOfWork.Save();
                TempData["success"] = "Quantity increased in Cart.";
            }
            return RedirectToAction("Index");
        }
        public IActionResult Decrease(int ProductId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            String userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCart cartObj = _unitOfWork.shoppingCart.FirstOrDefault(u => u.ProductId == ProductId && u.ApplicationUserId == userId);

            if (cartObj != null)
            {
                if (cartObj.Quantity <= 1)
                {
                    _unitOfWork.shoppingCart.Delete(cartObj);
                    TempData["success"] = "Item Deleted successfully.";
                }
                else
                {
                    cartObj.Quantity--;
                    TempData["success"] = "Quantity Decreased in Item.";

                }
                _unitOfWork.Save();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Remove(int ProductId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            String userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCart cartObj = _unitOfWork.shoppingCart.FirstOrDefault(u => u.ProductId == ProductId && u.ApplicationUserId == userId);

            if (cartObj != null)
            {

                _unitOfWork.shoppingCart.Delete(cartObj);
                TempData["success"] = "Item Removed from cart.";

                _unitOfWork.Save();
            }
            return RedirectToAction("Index");

        }
        public IActionResult CheckOut()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            String userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = _unitOfWork.applicationUser.FirstOrDefault(u => u.Id == userId);
             CartVMObj = new()
            {
                OrderHeader = new()
                {
                    ApplicationUserId = userId,
                    ApplicationUser= user,
                    Name=user.Name,
                    PhoneNumber=user.PhoneNumber,
                    StreetAddress=user.Street,
                    City=user.City,
                    State=user.State,
                    PostalCode=user.PostalCode,
                    Country=user.Country,

                },
                shoppingCarts = _unitOfWork.shoppingCart.GetAll(u => u.ApplicationUserId == userId, "Product").ToList(),
            };
            return View(CartVMObj);
        }
        [HttpPost]
        [ActionName("Checkout")]
        public IActionResult CheckOutPost (ShoppingCartViewModel CartVMObj)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            String userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = _unitOfWork.applicationUser.FirstOrDefault(u => u.Id == userId);
            CartVMObj.OrderHeader.ApplicationUserId = user.Id;
            CartVMObj.shoppingCarts = _unitOfWork.shoppingCart.GetAll(u => u.ApplicationUserId == userId, "Product").ToList();
            
            CartVMObj.OrderHeader.OrderDate = DateTime.Now;
            CartVMObj.OrderHeader.PaymentStatus = StaticData.PAYMENT_STATUS_PENDING;
            CartVMObj.OrderHeader.OrderStatus = StaticData.ORDER_STATUS_PENDING;

            foreach (var cart in CartVMObj.shoppingCarts)
            {
                CartVMObj.OrderHeader.OrderTotal += (cart.Product.Price * cart.Quantity);
            }
            _unitOfWork.orderHeader.Create(CartVMObj.OrderHeader);

            _unitOfWork.Save();
            foreach(var item in CartVMObj.shoppingCarts)
            {
                OrderDetails orderDetails = new()
                {
                    ProductID = item.ProductId,
                    OrderHeaderID = CartVMObj.OrderHeader.Id,
                    Quantity = item.Quantity,
                   Price=item.Product.Price,
                };
                _unitOfWork.orderDetails.Create(orderDetails);
                _unitOfWork.Save();
            }
            //stripe logic
            var domain = "https://localhost:7031/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"customer/shoppingcart/OrderConfirmation?id={CartVMObj.OrderHeader.Id}",
                CancelUrl = domain + "customer/order/index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };
            foreach(var item in CartVMObj.shoppingCarts)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price * 100),
                        Currency = "npr",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        }
                    },
                    Quantity = item.Quantity,

                };
                options.LineItems.Add(sessionLineItem);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            _unitOfWork.orderHeader.UpdateStripePaymentID(CartVMObj.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _unitOfWork.Save();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.orderHeader.FirstOrDefault(u => u.Id == id, includeProperties: "ApplicationUser");

            var service=new SessionService();
            Session session = service.Get(orderHeader.SessionId);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.orderHeader.UpdateStripePaymentID(id,session.Id, session.PaymentIntentId);
                _unitOfWork.orderHeader.UpdateStatus(id, StaticData.ORDER_STATUS_PENDING, StaticData.PAYMENT_STATUS_COMPLETED);
                _unitOfWork.Save();
            }

/* remove item from shopping cart
 */
            List<ShoppingCart> shoppingCarts = _unitOfWork.shoppingCart.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
            _unitOfWork.shoppingCart.DeleteRange(shoppingCarts);
            _unitOfWork.Save();
            return View(id);
        }
        
    }
}