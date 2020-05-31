using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using My.FrameworkCore.Core.Extensions;

namespace My.FrameworkCore.UserAPI
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

            /*
            services.AddAuthentication("Bearer").AddIdentityServerAuthentication(options =>
            {
                options.Authority = "http://localhost:33065";//指定授权地址
                options.RequireHttpsMetadata = false;//没有证书不启用https
                options.ApiName = "UserAPI";//指定apiName的名字，与Config类中相同
            });
            */


            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseAuthentication();//添加认证中间件

            app.UseMvc();

            //var consulClient = new ConsulClient(p => { p.Address = new Uri($"http://127.0.0.1:8500"); });

            //var httpCheck = new AgentServiceCheck()
            //{
            //    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
            //    Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
            //    HTTP = $"http://localhost:33054/api/values/Health",//健康检查地址
            //    Timeout = TimeSpan.FromSeconds(5)
            //};

            //var registration = new AgentServiceRegistration()
            //{
            //    Checks = new[] { httpCheck },
            //    ID = "UserAPI_33054",
            //    Name = "UserAPI",
            //    Address = "http://localhost/",
            //    Port = 33054
            //};

            app.RegisterConsul(lifetime);

        }
    }
}
