using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myshop.Entities.Repositories;
using MyShop.Uitilities;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.AdminRole)]
    public class DashboardController : Controller
    {
        private IUnitofwork _Unitofwork;
        public DashboardController(IUnitofwork unitofwork)
        {
            _Unitofwork = unitofwork;
        }
        public IActionResult Index()
        {
            ViewBag.Orders=_Unitofwork.headerRepository.GetAll().Count();
            ViewBag.ApprovedOrders= _Unitofwork.headerRepository.GetAll(x=>x.OrderSatus==SD.Approve).Count();
            ViewBag.Users= _Unitofwork.UserRepository.GetAll().Count();
            ViewBag.products= _Unitofwork.Product.GetAll().Count();

            return View();
        }
    }
}
