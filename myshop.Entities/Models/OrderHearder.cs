using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Models
{
	public class OrderHearder
	{
		public int Id { get; set; }

		[ForeignKey("ApplicationUserId")]
		[ValidateNever]
        public ApplicationUser  applicationUser{ get; set; }

          public string ApplicationUserId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
		public decimal TotalPrice { get; set; }
        public string? OrderSatus { get; set; }
		public string? PaymentStatus { get; set;}
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }
        public DateTime PaymentDate { get; set; }
        //stripe Properties
        public string? sessionId { get; set; }
        public string? PaymentIntendId { get; set;}
		//Data Of User
		public string Name { get; set; }

		public string Address { get; set; }
		public string City { get; set; }
		
		public string PhoneNumber { get; set; }

	}
}
