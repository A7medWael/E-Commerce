using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using myshop.Entities.Models;
using myshop.Entities.Repositories;
using myshop.Entities.ViewModels;
using MyShop.Entities.Models;
using NuGet.Packaging.Signing;


namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private IUnitofwork _Unitofwork;
        private IWebHostEnvironment _WebEnvironment;
        public ProductController(IUnitofwork unitofwork, IWebHostEnvironment webEnvironment)
        {
            _Unitofwork = unitofwork;
            _WebEnvironment = webEnvironment;
        }
        public IActionResult Index()
        {
            return View();
            //var product = _Unitofwork.Product.GetAll();
            //return View(product);

        }
        [HttpGet]
        public IActionResult GetData()
        {
            var pro = _Unitofwork.Product.GetAll(Includeword:"category");
            return Json(new {data= pro });
           
        }
        [HttpGet]
        public IActionResult Create()
        {
            ProductVm productVm = new ProductVm()
            {
                product = new Product(),
                CategoryList = _Unitofwork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(productVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVm productvm,IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string rootpath = _WebEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename=Guid.NewGuid().ToString();
                    var upload=Path.Combine(rootpath, "Images","Products");
                    //@"Images\Products"
                    var ext=Path.GetExtension(file.FileName);
                    using (var filestream = new FileStream(Path.Combine(upload, filename+ext),FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    productvm.product.Image = Path.Combine( "Images", "Products",filename + ext); 
                   // @"Images\Products\" + filename + ext;
                }
                //_context.Categories.Add(product);
                //_context.SaveChanges();
                _Unitofwork.Product.Add(productvm.product);
                _Unitofwork.complete();
                TempData["Create"] = "Data has created succesfully";
                return RedirectToAction("Index");
            }
            return View(productvm.product);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                NotFound();

            }
            ProductVm productVm = new ProductVm()
            {
                product = _Unitofwork.Product.GetFirstOrDefault(x => x.Id == id),
                CategoryList = _Unitofwork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })};
           
            return View(productVm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVm productvm,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string rootpath = _WebEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var upload = Path.Combine(rootpath, "Images", "Products");
                    //@"Images\Products"
                    var ext = Path.GetExtension(file.FileName);
                    if (productvm.product.Image != null)
                    {
                        var oldimg=Path.Combine(rootpath,productvm.product.Image.TrimStart('\\'));
                        if (System.IO.File.Exists(oldimg))
                        {
                            System.IO.File.Delete(oldimg);
                        }
                    }
                    using (var filestream = new FileStream(Path.Combine(upload, filename + ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    productvm.product.Image = Path.Combine("Images", "Products", filename + ext);
                    // @"Images\Products\" + filename + ext;
                }
                _Unitofwork.Product.Update(productvm.product);
                _Unitofwork.complete();
                TempData["Update"] = "Data has Updeted succesfully";
                return RedirectToAction("Index");
            }
            return View(productvm.product);
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            
            var prod = _Unitofwork.Product.GetFirstOrDefault(x => x.Id == id);
            if (prod == null)
            {
                Json(new { success = false, message = "Error while deleting" });
            }
            //_context.Categories.Remove(product);
            //_context.SaveChanges();
            _Unitofwork.Product.Remove(prod);
            var oldimg = Path.Combine(_WebEnvironment.WebRootPath, prod.Image.TrimStart('\\'));
            if (System.IO.File.Exists(oldimg))
            {
                System.IO.File.Delete(oldimg);
            }
            _Unitofwork.complete();
           return Json(new { success = true, message = "File has been deleted" });
            
        }
    }
}
