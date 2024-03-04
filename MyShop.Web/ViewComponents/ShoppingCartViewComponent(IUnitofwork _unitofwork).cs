using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using myshop.DataAccess.Implementation;
using myshop.Entities.Repositories;
using MyShop.Uitilities;
using System.Security.Claims;

namespace MyShop.Web.ViewComponents
{
	
	public class ShoppingCartViewComponent:ViewComponent
	{
		private readonly IUnitofwork unitofwork;
        public ShoppingCartViewComponent(IUnitofwork _unitofwork)
        {
            unitofwork = _unitofwork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
			var claimsidentity = (ClaimsIdentity)User.Identity;
			var claim = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
			if (claim != null)
			{
				if (HttpContext.Session.GetInt32(SD.SessionKey) != null)
				{
					return View(HttpContext.Session.GetInt32(SD.SessionKey));
				}
				else
				{
					HttpContext.Session.SetInt32(SD.SessionKey, unitofwork.ShoppingCart.GetAll(x => x.ApplicationUserId == claim.Value).ToList().Count());
					return View(HttpContext.Session.GetInt32(SD.SessionKey));
				}
			}
			else
			{
				HttpContext.Session.Clear();
				return View(0);
			}
		}
    }
}
