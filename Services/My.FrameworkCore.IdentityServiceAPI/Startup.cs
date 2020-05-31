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
using IdentityServer4;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Services;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using IdentityServer4.EntityFramework;
using MySql.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace My.FrameworkCore.IdentityServiceAPI
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

          var connectionString= Configuration.GetConnectionString("MallConnectionString");


            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";
                options.Authority = "http://localhost:33065";
                options.RequireHttpsMetadata = false;
                options.ClientId = "postman";
                options.ClientSecret = "secret";
                options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Add("api1");
                options.Scope.Add("offline_access");

                //options.ClaimActions.MapJsonKey("sub", "sub");
                //options.ClaimActions.MapJsonKey("preferred_username", "preferred_username");
                //options.ClaimActions.MapJsonKey("avatar", "avatar");

                options.SaveTokens = true;
                options.Scope.Add("profile");
                options.Scope.Add("openid");

            });

            //Microsoft.EntityFrameworkCore.Tools

            //注入IdentityServer4
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
                .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
                .AddInMemoryClients(IdentityServerConfig.GetClients())
                .AddResourceOwnerValidator<LoginResourceOwnerValidator>() //用户校验
                .AddProfileService<MyProfileService>()
                //.AddAspNetIdentity<ApplicationUser>() // 使用自定义用户
                //.AddConfigurationStore(options =>
                //{
                //    options.ConfigureDbContext = builder =>
                //    {
                //        builder.UseMySQL(connectionString, sql=> sql.MigrationsAssembly(""));
                //    };
                //})
                //.AddOperationalStore(options =>
                //{
                //    options.ConfigureDbContext = builder =>
                //    {
                //        builder.UseMySQL(connectionString, sql => sql.MigrationsAssembly(""));
                //    };
                //})
                .AddTestUsers(IdentityServerConfig.GetTestUsers());

            //services.AddScoped<IProfileService, MyProfileService>();

            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //启用认证
            //app.UseAuthentication();

            //启动IdentityServer4
            app.UseIdentityServer();

            //app.UseMvc();


        }
    }
}
