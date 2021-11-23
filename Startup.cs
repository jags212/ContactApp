using AutoMapper;
using ContactApp.Data;
using ContactApp.Data.Models;
using ContactApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ContactApp
{
    public class Startup
    {
        public readonly IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ContactContext>();

            services.AddAuthentication()
              .AddCookie()
              .AddJwtBearer(cfg =>
              {
                  cfg.TokenValidationParameters = new TokenValidationParameters()
                  {
                      ValidIssuer = Configuration["Tokens:Issuer"],
                      ValidAudience = Configuration["Tokens:Audience"],
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]))
                  };
              });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpClient<ServiceClient>();
            services.AddHttpClient<TokenClient>();

            services.AddDbContext<ContactContext>();

            services.AddTransient<ContactSeeder>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddControllersWithViews()
              .AddRazorRuntimeCompilation()
              .AddNewtonsoftJson(cfg => cfg.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddRazorPages();
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
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(cfg =>
            {
                cfg.MapControllerRoute("Fallback",
                  "{controller}/{action}/{id?}",
                  new { controller = "Home", action = "Index" });

                cfg.MapRazorPages();
            });
        }
    }
}
