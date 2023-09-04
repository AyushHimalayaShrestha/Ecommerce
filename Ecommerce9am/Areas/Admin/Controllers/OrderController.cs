using Ecommerce9am.Data.Repository;
using Ecommerce9am.Data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace Ecommerce9am.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;


        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult OrderDetails(int orderId)
        {
            OrderViewModel order = new()
            {
                OrderHeader = _unitOfWork.orderHeader.FirstOrDefault(u => u.Id == orderId),
                OrderDetails = _unitOfWork.orderDetails.GetAll(u => u.OrderHeaderID == orderId, includeProperties: "Product").ToList(),
            };
            return View(order);
        }


        #region-API Calls
        [HttpGet]
        [Authorize]
        public IActionResult GetAllOrders()
        {
            List<OrderHeader>orderHeaders=_unitOfWork.orderHeader.GetAll(includeProperties:"ApplicationUser").ToList();
            return Json(new {Data=orderHeaders});
        }
        #endregion


    }
}
