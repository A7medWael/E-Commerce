using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Repositories
{
    public interface IUnitofwork:IDisposable
    {
      public  ICategoryRepository Category { get; }
      public  IProductRepository Product { get; }
      public  IShoppingCartRepository ShoppingCart { get; }
      public  IOrderHeaderRepository  headerRepository { get; }
      public IOrderDetailsRepository DetailsRepository  { get; }
      public IApplicationUserRepository  UserRepository { get; }

        int complete();
    }
}
