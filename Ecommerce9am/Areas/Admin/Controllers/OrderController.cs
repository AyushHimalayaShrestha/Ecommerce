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
        public IActionResult GetAllOrders(string status)
        {
            List<OrderHeader> orderHeaders;
            if (status == "all")
            {

            orderHeaders=_unitOfWork.orderHeader.GetAll(includeProperties:"ApplicationUser").ToList();
            }
            else
            {
                 orderHeaders = _unitOfWork.orderHeader.GetAll(u => u.OrderStatus == status, includeProperties: "ApplicationUser").ToList();

            }
            return Json(new {Data=orderHeaders});
        }
        #endregion


    }
}
