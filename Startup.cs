using CrudApplication.Data;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace CrudApplication
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
            services.AddControllersWithViews();
            services.AddDbContext<ApplicationContext>(option => {
                option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option =>
                {
                    option.ExpireTimeSpan = TimeSpan.FromMinutes(60 * 1);
                    option.LoginPath = "/Account/Login";
                    option.AccessDeniedPath = "/Account/Login";
                });
            services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromMinutes(5);
                option.Cookie.HttpOnly = true;
                option.Cookie.IsEssential = true;

            });

            services.AddDNTCaptcha(option => {
                option.UseCookieStorageProvider().ShowThousandsSeparators(false);
                option.WithEncryptionKey("abdDFKDFDFjkjkdfjdkfj121212");
            
            });
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
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Laptop}/{action=Index}/{id?}");
            });
        }
    }
}
