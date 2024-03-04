using Microsoft.EntityFrameworkCore;
using myshop.DataAccess.Implementation;
using myshop.Entities.Repositories;
using MyShop.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyShop.Uitilities;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using myshop.DataAccess.DbIntializer;


namespace MyShop.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
            builder.Services.AddDbContext<ApplicationDbContext>(opt =>opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
            builder.Services.Configure<Stripedt>(builder.Configuration.GetSection("stripe"));

            builder.Services.AddIdentity<IdentityUser,IdentityRole>(opt=>opt.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromDays(4))
                .AddDefaultTokenProviders().AddDefaultUI()//<--ليها علاقة بالconfirmation
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddSingleton<IEmailSender,EmailSender>();
            builder.Services.AddScoped<IUnitofwork,Unitofwork>();
            builder.Services.AddScoped<IDbIntializer, DbIntializer>();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
           
              var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("stripe:Secretkey").Get<string>();
            SeedDb();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

            app.Run();

            void SeedDb()
            {
                using(var scope = app.Services.CreateScope())
                {
                    var dbint=scope.ServiceProvider.GetRequiredService<IDbIntializer>();
                    dbint.Intialize();
                }
            }










        }
    }
}
