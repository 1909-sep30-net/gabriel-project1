using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoapSoap.DataAccess.Entities;
using DoapSoap.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DoapSoap.DataAccess.Repositories;
using DoapSoap.WebApp.Models;

namespace DoapSoap.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DoapSoapDb");
            services.AddDbContext<DoapSoapContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            // this registers the Repository class under the "name" of IRepository.
            // aka: "if anyone needs an IRepository, make a Repository."
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();

            services.AddSingleton<CartViewModel>();

            services.AddSession();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
