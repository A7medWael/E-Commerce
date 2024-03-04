using myshop.Entities.Models;
using myshop.Entities.Repositories;
using MyShop.DataAccess;

namespace myshop.DataAccess.Implementation
{
    public class ProductRepository : GenericRepository<Product>,IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product product)
        {
            var ProductIndb=_context.Products.FirstOrDefault(x => x.Id == product.Id);
            if (ProductIndb != null)
            {
                ProductIndb.Name = product.Name;
                ProductIndb.Description = product.Description;
                    ProductIndb.Price = product.Price;
                ProductIndb.Image = product.Image;
                ProductIndb.CategoryId = product.CategoryId;


            }
        }
    }
}
