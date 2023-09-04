using Ecommerce9am.Data;
using Ecommerce9am.Data.Repository;
using Ecommerce9am.Data.Repository.IRepository;
using Ecommerce9am.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utils;

namespace Ecommerce9am.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =StaticData.ROLE_ADMIN)]
    public class CategoryController : Controller
    {
        //  private readonly ApplicationDBContext _db;
        //private readonly ICategoryRepository _db;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            //_db = db;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //IEnumerable<Category> categories = _db.GetAll();
            IEnumerable<Category> categories = _unitOfWork.Category.GetAll();

            return View(categories);
        }
        [HttpGet]
        public IActionResult PostCategory()
        {
            return View();
        }

        [HttpPost]

        public IActionResult PostCategory(Category productObj)
        {
            if (ModelState.IsValid)
            {
                if (productObj.CategoryName.ToUpper() == "TEST")
                {
                    ModelState.AddModelError("CategoryName", "Test is not a valid Category");
                    return View(productObj);
                }

                // _db.Create(productObj);
                _unitOfWork.Category.Create(productObj);
                //_db.Save();
                _unitOfWork.Save();
                TempData["success"] = "Category Added Successfully";
                return RedirectToAction("Index");

            }
            return View(productObj);
        }
        [HttpGet]
        public IActionResult UpdateCategory(int id)
        {
            //Category productToUpdate = _db.FirstOrDefault(u=>u.CategoryID==id);
            Category productToUpdate = _unitOfWork.Category.FirstOrDefault(u => u.CategoryID == id);
            if (productToUpdate == null)
            {
                return NotFound();
            }
            return View(productToUpdate);
        }
        [HttpPost]
        public IActionResult UpdateCategory(Category productToUpdate)
        {
            if (ModelState.IsValid)
            {
                //_db.Categories.Update(productToUpdate);
                //_db.Update(productToUpdate);

                _unitOfWork.Category.Update(productToUpdate);

                _unitOfWork.Save();

                //_db.SaveChanges();
                _unitOfWork.Save();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(productToUpdate);
        }
        [HttpGet]
        public IActionResult DeleteCategory(int id)
        {
            Category productToDelete = _unitOfWork.Category.FirstOrDefault(u => u.CategoryID == id);
            if (productToDelete == null)
            {
                return NotFound();
            }
            return View(productToDelete);
        }

        [HttpPost]
        [ActionName("DeleteCategory")]
        public IActionResult Delete(int id)
        {
            //Category productToDelete = _db.FirstOrDefault(u=>u.CategoryID==id);
            Category productToDelete = _unitOfWork.Category.FirstOrDefault(u => u.CategoryID == id);
            if (productToDelete == null)
            {
                TempData["success"] = "Delete failed";
                return NotFound();
            }
            _unitOfWork.Category.Delete(productToDelete);
            // _db.SaveChanges();
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");
        }
    }


}
