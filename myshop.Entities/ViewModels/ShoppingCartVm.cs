using myshop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.ViewModels
{
	public class ShoppingCartVm
	{
        public IEnumerable<ShoppingCart> cartlist { get; set; }
        public decimal totalcarts { get; set; }
        public OrderHearder orderhearder { get; set; }
    }
}
