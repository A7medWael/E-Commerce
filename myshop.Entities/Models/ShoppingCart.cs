using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace myshop.Entities.Models
{
    public class ShoppingCart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProductId { get; set; }

        //الاول بنعمل الجزء داعشان الdetails
        //مش بنستخدمه فى الداتا بيز ساعتها 
          [ForeignKey("ProductId")]
        [ValidateNever]
        public Product products { get; set; }
        
        [Range(1, 100, ErrorMessage = "You must enter value between 1 to 100")]
        public int Counts { get; set; }
        //لحد هنا
       [ForeignKey("ApplicationUserId")]
         [ValidateNever]
        public ApplicationUser  applicationUser{ get; set; }
        
        public string ApplicationUserId { get; set; }
    }
}
