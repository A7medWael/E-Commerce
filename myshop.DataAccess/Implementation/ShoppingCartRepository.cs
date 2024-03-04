using myshop.Entities.Models;
using myshop.Entities.Repositories;
using MyShop.DataAccess;
using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Implementation
{
    public class ShoppingCartRepository : GenericRepository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;
        public ShoppingCartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public int Decreascount(ShoppingCart cart, int count)
        {
            cart.Counts-=count;
            return cart.Counts;
        }

        public int Increascount(ShoppingCart cart, int count)
        {
            cart.Counts += count;
            return cart.Counts;
        }
    }
}
