using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using IdentityServer4;
using IdentityServer4.AccessTokenValidation;
using Consul;
using Ocelot.Provider.Consul;

namespace My.FrameworkCore.APIGateway
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
            var identityBuilder = services.AddAuthentication();

            identityBuilder.AddIdentityServerAuthentication("UserAPI", options =>
            {
                options.Authority = "http://localhost:33065";//指定授权地址
                options.RequireHttpsMetadata = false;//没有证书不启用https
                options.ApiName = "UserAPI";         //指定apiName的名字，与Config类中相同
                options.SupportedTokens = SupportedTokens.Both;
            });


            identityBuilder.AddIdentityServerAuthentication("OrderAPI", options =>
            {
                options.Authority = "http://localhost:33065";//指定授权地址
                options.RequireHttpsMetadata = false;//没有证书不启用https
                options.ApiName = "OrderAPI";        //指定apiName的名字，与Config类中相同
                options.SupportedTokens = SupportedTokens.Both;
            });

            services.AddMvc();

            //添加Ocelot
            //services.AddOcelot(Configuration);
            //services.AddOcelot(Configuration).AddConsul();
            services.AddOcelot(Configuration).AddConsul().AddConfigStoredInConsul();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMvc();

            //添加Ocelot依赖注入中间件
            app.UseOcelot().Wait();
        }
    }
}
