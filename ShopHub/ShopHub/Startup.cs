using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShopHub.Model.Context;
using ShopHub.Service.AutoMapper;
using ShopHub.Service.Interface;
using ShopHub.Service.Services;

namespace ShopHub
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //Use this Interface "services" to add MVC core DI container + All below can now use DI 
        //@Inject in the Views or just Field instances in Inpl classes I.E private ShopHubContext _context;
        public void ConfigureServices(IServiceCollection services)
        { 
            services.AddControllersWithViews();
            //Singleton-Single instance is used for scope of the http request + across different http requests
            //Single Instance throughout the entire app.. same instance is re-used
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); 
            //Add-Scoped - Single instance is re-used for scope of this http request only
            //A new instance is created across different http request
            services.AddScoped<ISessionManager, SessionManager>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILocation, LocationService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();
            //AddTransient - A new instance is created for the scope of each and every http request 
            services.AddTransient<ShopHubContext>();
            services.AddSession(options =>  ///Options to create a new cookie
            {
                options.Cookie.Name = ".ShopHub.Session";
                options.IdleTimeout = TimeSpan.FromDays(10);
                options.Cookie.IsEssential = true;      //Essential for app to work...policy checks may be bypassed ...store in url vs cookie file
                options.Cookie.MaxAge = TimeSpan.FromDays(10);
            });

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapping());   //Maps the objects in DB context to DTO objects
            });                                     //Property names are same + extra's to make binding easier in view's

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
        }

        // Called at runtime. Configures the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days.  https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();       //Register session with the app
            app.UseAuthorization();

            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=AuthUser}/{action=Login}/{id?}");
            });
        }
    }
}
