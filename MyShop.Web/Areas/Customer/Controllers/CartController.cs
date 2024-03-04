using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using myshop.DataAccess.Implementation;
using myshop.Entities.Models;
using myshop.Entities.Repositories;
using myshop.Entities.ViewModels;
using MyShop.Uitilities;
using Stripe.Checkout;
using System.Security.Claims;

namespace MyShop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitofwork _unitofwork;
        ShoppingCartVm shoppingCartVm { get; set; }
        public int TotalCart { get; set; }
        public CartController(IUnitofwork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public IActionResult Index()
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartVm = new ShoppingCartVm()
            {
                cartlist = _unitofwork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, Includeword: "products"),
                orderhearder = new()
            };
            foreach (var item in shoppingCartVm.cartlist)
            {
                shoppingCartVm.totalcarts += (item.Counts * item.products.Price);
            }
           
            return View(shoppingCartVm);
        }
        public IActionResult Plus(int cartid)

        {
            var shoppingcart=_unitofwork.ShoppingCart.GetFirstOrDefault(x=>x.Id==cartid);
            _unitofwork.ShoppingCart.Increascount(shoppingcart, 1);
            _unitofwork.complete();
            return RedirectToAction("Index");
        }
		public IActionResult Minus(int cartid)

		{
			var shoppingcart = _unitofwork.ShoppingCart.GetFirstOrDefault(x => x.Id == cartid);
            if(shoppingcart.Counts<= 1)
            {
                _unitofwork.ShoppingCart.Remove(shoppingcart);
                var count = _unitofwork.ShoppingCart.GetAll(x => x.ApplicationUserId == shoppingcart.ApplicationUserId).ToList().Count()-1;
                HttpContext.Session.SetInt32(SD.SessionKey, count);
				//_unitofwork.complete();
				//return RedirectToAction("Index","Home");


			}
            else
            {
                 _unitofwork.ShoppingCart.Decreascount(shoppingcart, 1);
            }
			
			_unitofwork.complete();
			return RedirectToAction("Index");
		}

        public IActionResult Remove(int cartid)

        {
            var shoppingcart = _unitofwork.ShoppingCart.GetFirstOrDefault(x => x.Id == cartid);
          
                _unitofwork.ShoppingCart.Remove(shoppingcart);
                _unitofwork.complete();
			var cont = _unitofwork.ShoppingCart.GetAll(x => x.ApplicationUserId == shoppingcart.ApplicationUserId).ToList().Count();
			HttpContext.Session.SetInt32(SD.SessionKey, cont);
			return RedirectToAction("Index");


            
        }
        [HttpGet]
		public IActionResult Summary()
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartVm = new ShoppingCartVm()
            {
                cartlist = _unitofwork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, Includeword: "products"),
                orderhearder=new()
                
            };
            shoppingCartVm.orderhearder.applicationUser = _unitofwork.UserRepository.GetFirstOrDefault(x => x.Id == claim.Value);
            shoppingCartVm.orderhearder.Name=shoppingCartVm.orderhearder.applicationUser.Name;
            shoppingCartVm.orderhearder.Address = shoppingCartVm.orderhearder.applicationUser.Address;
            shoppingCartVm.orderhearder.City = shoppingCartVm.orderhearder.applicationUser.City;
            shoppingCartVm.orderhearder.PhoneNumber = shoppingCartVm.orderhearder.applicationUser.PhoneNumber;
      foreach (var item in shoppingCartVm.cartlist)
            {
                shoppingCartVm.orderhearder.TotalPrice += (item.Counts * item.products.Price);
            }
            return View(shoppingCartVm);
           
        }
        //[ActionName("Summary")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PostSummary(ShoppingCartVm shoppingCartVm)
        {
            var claimsidentity = (ClaimsIdentity)User.Identity;
            var claim = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCartVm.cartlist = _unitofwork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, Includeword: "products");
               

           
           
            shoppingCartVm.orderhearder.OrderSatus = SD.Pending;
            shoppingCartVm.orderhearder.PaymentStatus = SD.Pending;
            shoppingCartVm.orderhearder.OrderDate = DateTime.Now;
            shoppingCartVm.orderhearder.ApplicationUserId = claim.Value;
           
            foreach (var item in shoppingCartVm.cartlist)
            {
                shoppingCartVm.orderhearder.TotalPrice += (item.Counts * item.products.Price);
            }
            _unitofwork.headerRepository.Add(shoppingCartVm.orderhearder);
             _unitofwork.complete();
            foreach(var i in shoppingCartVm.cartlist)
            {
                OrderDetails orderDetails = new OrderDetails()
                {
                    ProductId = i.ProductId,
                    OrderId = shoppingCartVm.orderhearder.Id,
                    Price = i.products.Price,
                    count = i.Counts
                };
                _unitofwork.DetailsRepository.Add(orderDetails);
                _unitofwork.complete();
            }
            
            var domain = "https://myshopweb20240224201343.azurewebsites.net/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
        
                Mode = "payment",
                SuccessUrl = domain+$"Customer/cart/Orderconfirmation?id={shoppingCartVm.orderhearder.Id}",
                CancelUrl = domain+$"Customer/cart/Index",
            };
            foreach(var item in shoppingCartVm.cartlist)
            {
               var sessionline= new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.products.Price*100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.products.Name,
                        },
                    },
                    Quantity = item.Counts,
                };
               options.LineItems.Add(sessionline);
        }

            var service = new SessionService();
            Session session = service.Create(options);
            shoppingCartVm.orderhearder.sessionId = session.Id;
           // shoppingCartVm.orderhearder.PaymentIntendId = session.PaymentIntentId;
            _unitofwork.complete();
            
            Response.Headers.Add("Location",session.Url);

            return new StatusCodeResult(303);

        }
        public IActionResult Orderconfirmation(int id)
        {
            OrderHearder orderHearder= _unitofwork.headerRepository.GetFirstOrDefault(u=>u.Id==id);
            var service = new SessionService();
            Session session = service.Get(orderHearder.sessionId);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitofwork.headerRepository.updateorderstatus(id,SD.Approve,SD.Approve);
                orderHearder.PaymentIntendId = session.PaymentIntentId;
                _unitofwork.complete();
            }
            List<ShoppingCart> shoppingCarts = _unitofwork.ShoppingCart.GetAll(x => x.ApplicationUserId == orderHearder.ApplicationUserId).ToList();
            
            _unitofwork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitofwork.complete();
            return View();/*RedirectToAction("Index", "Home");*/

        }

    }
}
        