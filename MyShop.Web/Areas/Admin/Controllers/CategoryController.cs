using Microsoft.AspNetCore.Mvc;
using myshop.Entities.Repositories;
using MyShop.Entities.Models;


namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private IUnitofwork _Unitofwork;
        public CategoryController(IUnitofwork unitofwork)
        {
            _Unitofwork = unitofwork;
        }
        public IActionResult Index()
        {
            var category = _Unitofwork.Category.GetAll();
            return View(category);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                //_context.Categories.Add(category);
                //_context.SaveChanges();
                _Unitofwork.Category.Add(category);
                _Unitofwork.complete();
                TempData["Create"] = "Data has created succesfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                NotFound();

            }
            var category = _Unitofwork.Category.GetFirstOrDefault(x => x.Id == id);
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _Unitofwork.Category.Update(category);
                _Unitofwork.complete();
                TempData["Update"] = "Data has Updeted succesfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                NotFound();

            }
            var category = _Unitofwork.Category.GetFirstOrDefault(x => x.Id == id);
            return View(category);
        }

        public IActionResult Deleteitem(int? id)
        {
            var category = _Unitofwork.Category.GetFirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                NotFound();
            }
            //_context.Categories.Remove(category);
            //_context.SaveChanges();
            _Unitofwork.Category.Remove(category);
            _Unitofwork.complete();
            TempData["Delete"] = "Data has deleted succesfully";
            return RedirectToAction("Index");
        }
    }
}
