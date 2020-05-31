using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using My.FrameworkCore.Models.Domain;
using My.FrameworkCore.Services;
using My.FrameworkCore.Services.Implement;
using My.FrameworkCore.Services.Interface;
using My.FrameworkCore.Core;
using My.FrameworkCore.Core.Extensions;


namespace My.FrameworkCore.Web
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
            services.AddDbContext<MallDbContext>(d => d.UseMySQL(Configuration.GetConnectionString("MallConnectionString")));


           

            //泛型注入到DI里面
            services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));

            services.AddAssembly("My.FrameworkCore.Services");

            //services.AddScoped<IUserService, UserService>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
