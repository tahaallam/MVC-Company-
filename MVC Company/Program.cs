using Company.Data.Contexts;
using Company.Data.Models;
using Company.Repository.Interfaces;
using Company.Repository.Repositories;
using Company.Services.Interfaces;
using Company.Services.Profiles;
using Company.Services.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MVC_Company
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
               options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });
           builder.Services.AddScoped<IUnitOfWork , UnitOfWork>();
           builder.Services.AddScoped<IDepartmentServices, DepartmentServices>();
           builder.Services.AddScoped<IEmployeeServices, EmployeeServices>();
            builder.Services.AddAutoMapper(X => X.AddProfile(new EmployeeProfile()));
            builder.Services.AddAutoMapper(X => X.AddProfile(new DepartmentProfile()));

            //  builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            // builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            // builder.Services.AddScoped<IGenericRepository<BaseEntity>, GenericRepository<BaseEntity>>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequiredUniqueChars= 2;
                config.Password.RequireDigit= true;
                config.Password.RequireLowercase= true;
                config.Password.RequireUppercase= true;
                config.Password.RequireNonAlphanumeric= true;
                config.User.RequireUniqueEmail= true;   
                config.Lockout.AllowedForNewUsers= true;
                config.Lockout.MaxFailedAccessAttempts= 3;
                config.Lockout.DefaultLockoutTimeSpan= TimeSpan.FromMinutes(3);

            }).AddEntityFrameworkStores<CompanyDbContext>().AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;

            });

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Register}/{id?}");

            app.Run();
           
        }
    }
}
