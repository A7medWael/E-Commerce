using myshop.Entities.Repositories;
using MyShop.DataAccess;
using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.Implementation
{
    public class CategoryRepository : GenericRepository<Category>,ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Category category)
        {
            var categoryIndb=_context.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (categoryIndb != null)
            {
                categoryIndb.Name = category.Name;
                categoryIndb.Description = category.Description;
                categoryIndb.CreatedTime = DateTime.Now;
            }
        }
    }
}
