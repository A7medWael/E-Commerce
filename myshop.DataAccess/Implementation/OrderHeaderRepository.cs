using myshop.Entities.Models;
using myshop.Entities.Repositories;
using MyShop.DataAccess;
using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Implementation
{
    public class OrderHeaderRepository : GenericRepository<OrderHearder>, IOrderHeaderRepository
	{
        private readonly ApplicationDbContext _context;
		public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(OrderHearder orderHearder)
        {
            _context.OrderHearders.Update(orderHearder);
        }

		public void updateorderstatus(int id, string OrderSatus, string PaymentStatus)
		{
			var orderfromdb=_context.OrderHearders.FirstOrDefault(x => x.Id == id);
            if(orderfromdb != null)
            {
                orderfromdb.OrderSatus=OrderSatus;
                orderfromdb.PaymentDate = DateTime.Now;
                if (PaymentStatus != null)
                {
                    orderfromdb.PaymentStatus=PaymentStatus;
                }
            }
		}
	}
}
