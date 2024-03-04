using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using myshop.Entities.Models;
using myshop.Entities.Repositories;
using MyShop.Uitilities;
using PagedList.Core;
using System.Security.Claims;

namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
   
    public class HomeController : Controller
    {
        private readonly IUnitofwork unitofwork;
        public HomeController(IUnitofwork _unitofwork)
        {
            unitofwork = _unitofwork;
        }
        public IActionResult Index(int ? page)
        {
            //var PageNmber=page ?? 1;
            //int PageSize = 8;
            var products = unitofwork.Product.GetAll();/*.ToPagedList(PageNmber,PageSize);*/
            return View(products);
        }
        
        public IActionResult Details(int ProductId)
        {
            ShoppingCart obj = new ShoppingCart()
            {
                ProductId = ProductId,
                products = unitofwork.Product.GetFirstOrDefault(x => x.Id == ProductId, Includeword: "category"),
                Counts = 1
            };
            return View(obj);
        
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shopping)
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            shopping.ApplicationUserId = claim.Value;
            ShoppingCart cartobj=unitofwork.ShoppingCart.GetFirstOrDefault(x => x.ApplicationUserId==claim.Value &&x.ProductId==shopping.ProductId);
            if (cartobj == null)
            {
               unitofwork.ShoppingCart.Add(shopping);
                     unitofwork.complete();
                HttpContext.Session.SetInt32(SD.SessionKey,
                    unitofwork.ShoppingCart.GetAll(x => x.ApplicationUserId == claim.Value).ToList().Count()
                    );
                

            }
            else
            {
                unitofwork.ShoppingCart.Increascount(cartobj, shopping.Counts);
                unitofwork.complete();
            }
          
                 
            return RedirectToAction("Index");

        }
    }
}
