using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using myshop.Entities.Models;
using MyShop.Entities.Models;

namespace MyShop.DataAccess
{
    public class ApplicationDbContext:IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        public DbSet<Category> Categories{ get; set; }
        public DbSet<Product> Products{ get; set; }
        public DbSet<ShoppingCart>  ShoppingCarts{ get; set; }
        public DbSet<ApplicationUser>  applicationUsers{ get; set; }
        public DbSet<OrderHearder>  OrderHearders{ get; set; }
        public DbSet<OrderDetails>  OrderDetails{ get; set; }
    }
}
