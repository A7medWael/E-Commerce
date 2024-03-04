using myshop.Entities.Repositories;
using MyShop.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Implementation
{
    public class Unitofwork : IUnitofwork
    {
        private readonly ApplicationDbContext _context;
        public ICategoryRepository Category { get; private set; }

        public IProductRepository Product { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IOrderHeaderRepository headerRepository{ get; private set; }
                                                             
        public IOrderDetailsRepository DetailsRepository{ get; private set; }
                                                            
        public IApplicationUserRepository UserRepository { get; private set; }

        public Unitofwork(ApplicationDbContext context) 
        {
            _context = context;
            Category = new CategoryRepository(context);
            Product = new ProductRepository(context);
            ShoppingCart = new ShoppingCartRepository(context);
            headerRepository = new OrderHeaderRepository(context);
            DetailsRepository=new OrderDetailsRepository(context);
            UserRepository = new ApplicationUserRepository(context);
        }
       
        public int complete()
        {
           
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
