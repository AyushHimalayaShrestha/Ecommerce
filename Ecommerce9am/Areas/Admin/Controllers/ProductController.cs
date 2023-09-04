using Ecommerce9am.Data;
using Ecommerce9am.Data.Repository;
using Ecommerce9am.Data.Repository.IRepository;
using Ecommerce9am.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Utils;

namespace Ecommerce9am.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticData.ROLE_ADMIN)]
    public class ProductController : Controller
    {
        //  private readonly ApplicationDBContext _db;
        //private readonly IProductRepository _db;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            //_db = db;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            //IEnumerable<Product> products = _db.GetAll();
            IEnumerable<Product> products = _unitOfWork.product.GetAll(includeProperties :"Category");

            return View(products);
        }
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.CategoryName,
                Value = u.CategoryID.ToString(),
            });
            if (id == null || id == 0)
            {
                ProductViewModel productViewModel = new ProductViewModel()
                {
                    product = new Product(),
                    categoryList = categoryList
                };

                return View(productViewModel);
            }
            else
            {
                ProductViewModel productViewModel = new ProductViewModel()
                {
                    product = _unitOfWork.product.FirstOrDefault(u => u.ID == id),
                    categoryList = categoryList
                };
                return View(productViewModel);
            }
        }

        [HttpPost]

        public IActionResult Upsert(ProductViewModel productObj, IFormFile? file)
        {

            string wwwroot = _webHostEnvironment.WebRootPath;
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetFileName(file.FileName) + Path.GetExtension(file.FileName);
                    string ProductPath = Path.Combine(wwwroot, @"Images/Products", filename);

                    if (!string.IsNullOrEmpty(productObj.product.ImageUrl))
                    {
                        var oldfile = Path.Combine(ProductPath, productObj.product.ImageUrl);
                        if (System.IO.File.Exists(oldfile))
                        {
                            System.IO.File.Delete(oldfile);
                        }

                    }

                    using (var fileStream = new FileStream(ProductPath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productObj.product.ImageUrl = filename;
                }
                if (productObj.product.ID == 0)
                {
                    _unitOfWork.product.Create(productObj.product);
                    TempData["success"] = "Product Added Successfully";

                }
                else
                {
                    _unitOfWork.product.Update(productObj.product);
                    TempData["success"] = "Product Updated Successfully";
                }


                //_db.Save();
                _unitOfWork.Save();
                return RedirectToAction("Index");

            }
            return View(productObj);
        }
        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {
            //Product productToUpdate = _db.FirstOrDefault(u=>u.ProductID==id);
            Product productToUpdate = _unitOfWork.product.FirstOrDefault(u => u.ID == id, "Category");
            if (productToUpdate == null)
            {
                return NotFound();
            }
            return View(productToUpdate);
        }
        [HttpPost]
        public IActionResult UpdateProduct(Product productToUpdate)
        {
            if (ModelState.IsValid)
            {
                //_db.Categories.Update(productToUpdate);
                //_db.Update(productToUpdate);

                _unitOfWork.product.Update(productToUpdate);

                _unitOfWork.Save();

                //_db.SaveChanges();
                _unitOfWork.Save();
                TempData["success"] = "Product Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(productToUpdate);
        }
        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            Product productToDelete = _unitOfWork.product.FirstOrDefault(u => u.ID == id, "Category");
            if (productToDelete == null)
            {
                return NotFound();
            }
            return View(productToDelete);
        }

        [HttpPost]
        [ActionName("DeleteProduct")]
        public IActionResult Delete(int id)
        {
            //Product productToDelete = _db.FirstOrDefault(u=>u.ProductID==id);
            Product productToDelete = _unitOfWork.product.FirstOrDefault(u => u.ID == id, "Category");
            if (productToDelete == null)
            {
                TempData["success"] = "Delete failed";
                return NotFound();
            }
            _unitOfWork.product.Delete(productToDelete);
            // _db.SaveChanges();
            _unitOfWork.Save();
            TempData["success"] = "Product Deleted Successfully";
            return RedirectToAction("Index");
        }
    }


}
