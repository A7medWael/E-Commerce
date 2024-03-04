using myshop.Entities.Models;
using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Repositories
{
    public interface IOrderHeaderRepository : IGenericRepository<OrderHearder>
    {
        void Update(OrderHearder orderHearder);
        void updateorderstatus(int id,string OrderSatus,string PaymentStatus);


    }
}
