using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myshop.DataAccess.Implementation;
using myshop.Entities.Models;
using myshop.Entities.Repositories;
using myshop.Entities.ViewModels;
using MyShop.Uitilities;
using Stripe;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.AdminRole)]
    public class OrderController : Controller
    {
        private readonly IUnitofwork _unitofwork;
        [BindProperty]
        public OrderVm orderVm  { get; set; }
        public OrderController(IUnitofwork unitofwork)
        {
            _unitofwork = unitofwork;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetData()
        {
            IEnumerable<OrderHearder> orderHearder;
            orderHearder = _unitofwork.headerRepository.GetAll(Includeword: "applicationUser");

            return Json(new { data = orderHearder });

        }
        [HttpGet]
        public IActionResult Details(int Orderid)
        {
            OrderVm vm = new OrderVm()
            {
                orderHearder = _unitofwork.headerRepository.GetFirstOrDefault(x => x.Id == Orderid, Includeword: "applicationUser"),
                orderDetails = _unitofwork.DetailsRepository.GetAll(x => x.OrderId == Orderid, Includeword: "product")
            };
    return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateDetails()
        {
            var order = _unitofwork.headerRepository.GetFirstOrDefault(x => x.Id == orderVm.orderHearder.Id);
            order.Name = orderVm.orderHearder.Name;
            order.PhoneNumber = orderVm.orderHearder.PhoneNumber;
            order.Address = orderVm.orderHearder.Address;
            order.City = orderVm.orderHearder.City;
            if (orderVm.orderHearder.Carrier != null)
            {
                order.Carrier=orderVm.orderHearder.Carrier;
            }
			if (orderVm.orderHearder.TrackingNumber != null)
			{
				order.TrackingNumber = orderVm.orderHearder.TrackingNumber;
			}
            _unitofwork.headerRepository.Update(order);
            _unitofwork.complete();
			TempData["Update"] = "Data has Updated succesfully";
            return RedirectToAction("Details", "Order", new { orderid = order.Id });

		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult StartProccess()
		{
            _unitofwork.headerRepository.updateorderstatus(orderVm.orderHearder.Id, SD.Proccessing, null);
            _unitofwork.complete();
			
			TempData["Update"] = "order status has Updated succesfully";
			return RedirectToAction("Details", "Order", new { orderid = orderVm.orderHearder.Id });

		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult StartShip()
		{
			var order = _unitofwork.headerRepository.GetFirstOrDefault(x => x.Id == orderVm.orderHearder.Id);
            order.TrackingNumber = orderVm.orderHearder.TrackingNumber;
            order.Carrier=orderVm.orderHearder.Carrier;
            order.OrderSatus = SD.Shipped;
            order.ShippingDate = DateTime.Now;
            _unitofwork.headerRepository.Update(order);
			
			_unitofwork.complete();

			TempData["Update"] = "order has shipped succesfully";
			return RedirectToAction("Details", "Order", new { orderid = orderVm.orderHearder.Id });

		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CancelOrder()
		{
			var order = _unitofwork.headerRepository.GetFirstOrDefault(x => x.Id == orderVm.orderHearder.Id);
            if (order.PaymentStatus == SD.Approve)
            {
                var option = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = order.PaymentIntendId
                };
                var service = new RefundService();
                Refund refund = service.Create(option);
                _unitofwork.headerRepository.updateorderstatus(order.Id, SD.Cancelled, SD.Refund);
            }
            else
            {
				_unitofwork.headerRepository.updateorderstatus(order.Id, SD.Cancelled, SD.Refund);
			}
            _unitofwork.complete();


			TempData["Update"] = "order  has Cancelled succesfully";
			return RedirectToAction("Details", "Order", new { orderid = orderVm.orderHearder.Id });

		}


	}
}
