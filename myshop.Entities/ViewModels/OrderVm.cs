using myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.ViewModels
{
    public class OrderVm
    {
        public OrderHearder  orderHearder { get; set; }
        public IEnumerable<OrderDetails> orderDetails { get; set; }
    }
}
