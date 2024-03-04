using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [ValidateNever]
        public string Image { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        [ValidateNever]
        public Category category { get; set; }
    }
}
