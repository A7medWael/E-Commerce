using myshop.Entities.Models;
using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Repositories
{
    public interface IOrderDetailsRepository : IGenericRepository<OrderDetails>
    {
        void Update(OrderDetails orderDetails);

    }
}
