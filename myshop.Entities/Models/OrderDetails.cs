using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Models
{
	public class OrderDetails
	{
        public int Id { get; set; }
        [ForeignKey("OrderId")]
		[ValidateNever]
        public OrderHearder  orderHearder { get; set; }
        public int OrderId { get; set; }
		[ForeignKey("ProductId")]
		[ValidateNever]
		public Product  product{ get; set; }
        public int ProductId { get; set; }
        public int count { get; set; }
		public decimal Price { get; set; }
	}
}
