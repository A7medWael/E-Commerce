﻿using System.ComponentModel.DataAnnotations;

namespace MyShop.Entities.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedTime { get; set; }=DateTime.Now;
    }
}
