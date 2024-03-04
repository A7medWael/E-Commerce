using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.DataAccess;
using MyShop.Uitilities;
using System.Security.Claims;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.AdminRole)]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var claimsidentity=(ClaimsIdentity)User.Identity;
            var claim=claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            string userid = claim.Value;
            return View(_context.applicationUsers.Where(x=>x.Id!=userid).ToList());
        }
        public IActionResult LockUnLock(string ? id)
        {
            var user=_context.applicationUsers.FirstOrDefault(x=>x.Id==id);
            if(user==null)
            {
              return  NotFound ();
            }else if(user.LockoutEnd==null||user.LockoutEnd<DateTime.Now)
            {
                user.LockoutEnd= DateTime.Now.AddYears(1);
            }
            else
            {
                user.LockoutEnd= DateTime.Now;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Users", new {area="Admin"});
        }
    }
}
