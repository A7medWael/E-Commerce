using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using myshop.Entities.Models;
using MyShop.DataAccess;
using MyShop.Uitilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.DataAccess.DbIntializer
{
    public class DbIntializer : IDbIntializer
    {
       
            


        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _applicationDbContext;
        public DbIntializer(
            UserManager<IdentityUser> userManager,
          
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext applicationDbContext
            )
        {
            _userManager = userManager;
            
            _roleManager = roleManager;
            _applicationDbContext = applicationDbContext;
        }

        public void Intialize()
        {
            //migration
            try
            {
                if(_applicationDbContext.Database.GetPendingMigrations().Count() > 0)
                {
                    _applicationDbContext.Database.Migrate();
                }

            }
            catch (Exception)
            {

                throw;
            }
            //roles
            if (!_roleManager.RoleExistsAsync(SD.AdminRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.EditorRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.CustomerRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole("Student")).GetAwaiter().GetResult();




                //user

                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName="Admin@myshop.com",
                    Email= "Admin@myshop.com",
                    PhoneNumber="01158864113",
                    Address="Gharbia",
                    City="El-Mahla"
                },"Ahmed12345?").GetAwaiter().GetResult();
                ApplicationUser user=_applicationDbContext.applicationUsers.FirstOrDefault(U=>U.Email== "Admin@myshop.com");
                _userManager.AddToRoleAsync(user, SD.AdminRole).GetAwaiter().GetResult();


            }

            return;
        }
    }
}
