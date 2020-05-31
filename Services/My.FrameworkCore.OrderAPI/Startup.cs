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

namespace My.FrameworkCore.OrderAPI
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
            services.AddMvc();

            //添加Swagger.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "DemoAPI", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            //配置Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DemoAPI V1");
            });


            ////注册Consul
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
            //    ID = "OrderAPI_33055",
            //    Name = "OrderAPI",
            //    Address = "http://localhost/",
            //    Port = 32534
            //};

            app.RegisterConsul(lifetime);

        }
    }
}
