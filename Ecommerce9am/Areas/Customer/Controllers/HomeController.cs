using Ecommerce9am.Data.Repository.IRepository;
using Ecommerce9am.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using System.Diagnostics;
using Utils;

namespace Ecommerce9am.Areas.Admin.Controllers
{
    [Area("Customer")]
    [Authorize(Roles =StaticData.ROLE_CUSTOMER)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [AllowAnonymous]
        public IActionResult ViewProducts()
        {
            IEnumerable<Product> products= _unitOfWork.product.GetAll();
            return View(products);
        }

        public IActionResult ViewDetails(int productId)
        {
            ShoppingCart shoppingCart = new()
            {
                ProductId = productId,
                Product = _unitOfWork.product.FirstOrDefault(u => u.ID == productId),
                Quantity = 1
            };
            return View(shoppingCart);
        }
    }
}