using Microsoft.Extensions.DependencyInjection;
using My.FrameworkCore.Core.Helper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Consul;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace My.FrameworkCore.Core.Extensions
{
    public static class ConsulServiceExtensions
    {
        public class ConsulServiceConfiguration
        {
            public ConsulClientConfiguration Client { set; get; }

            public ConsulServiceRegistration ServiceRegistration { set; get; }
        }

        public class ConsulClientConfiguration
        {
            public string Address { set; get; }
            public string Datacenter { set; get; }
            public string Token { set; get; }
            public double WaitTime { set; get; }
        }

        public class ConsulServiceRegistration
        {
            public string Id { set; get; }
            public string Name { set; get; }
            public string[] Tags { set; get; }
            public int Port { set; get; }
            public string Address { set; get; }
            public ConsulChecks[] Checks { set; get; }
        }

        public class ConsulChecks
        {
            /// <summary>
            /// 服务启动多久后注册
            /// </summary>
            public double DeregisterCriticalServiceAfter { set; get; }
            /// <summary>
            /// 健康检查时间间隔，或者称为心跳间隔
            /// </summary>
            public double Interval { set; get; }
            /// <summary>
            /// 健康检查超时时间
            /// </summary>
            public double Timeout { set; get; }
            /// <summary>
            /// 健康检查地址
            /// </summary>
            public string HealthCheckUrl { set; get; }

        }

        /// <summary>
        /// 注册Consul
        /// </summary>
        /// <param name="app"></param>
        /// <param name=""></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IApplicationLifetime lifetime)
        {

            //http://localhost:8500/v1/agent/checks

            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("consul.json");
            var consulConfig = builder.Build().Get<ConsulServiceConfiguration>();

            if (consulConfig == null)
            {
                throw new Exception("consul config is null");
            }

            var registration = new AgentServiceRegistration()
            {
                ID = consulConfig.ServiceRegistration.Id,
                Name = consulConfig.ServiceRegistration.Name,
                Address = consulConfig.ServiceRegistration.Address,
                Port = consulConfig.ServiceRegistration.Port,
            };

            //请求注册的 Consul 地址
            var consulClient = new ConsulClient(p =>
            {
                p.Address = new Uri(consulConfig.Client.Address);
                p.Datacenter = consulConfig.Client.Datacenter;
                p.Token = consulConfig.Client.Token;
                p.WaitTime = TimeSpan.FromSeconds(consulConfig.Client.WaitTime);
            });

            registration.Checks = new AgentServiceCheck[consulConfig.ServiceRegistration.Checks.Length];
            int i = 0;
            foreach (var item in consulConfig.ServiceRegistration.Checks)
            {
                //这里的这个ip 就是本机的ip，这个端口8500 这个是默认注册服务端口 
                registration.Checks[i] = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(item.DeregisterCriticalServiceAfter),//服务启动多久后注册
                    Interval = TimeSpan.FromSeconds(item.Interval),//健康检查时间间隔，或者称为心跳间隔
                    HTTP = item.HealthCheckUrl,//健康检查地址
                    Timeout = TimeSpan.FromSeconds(5) //健康检查超时时间
                };

                i++;
            }

            consulClient.Agent.ServiceRegister(registration).Wait();//注册服务 

            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();//服务停止时取消注册
            });

            return app;
        }
    }

 

}
