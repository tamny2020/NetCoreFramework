using Polly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace My.FrameworkCore.Core.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpClientRequestProxy
    {

        public static async Task<string> GetStringAsync(string url)
        {
            Policy.Handle<HttpRequestException>().Retry(3, (exception, retryCount) =>
            {
                // 干点什么，比如记个日志之类的 

            }).Execute(() =>
            {

            });

            // 重试3次，分别等待1、2、3秒。
            Policy.Handle<HttpRequestException>().WaitAndRetry(new[] {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(3)
            },
            (exception, timeSpan, context) =>
            {
                // 干点什么，比如记个日志之类的 

            }).Execute(() =>
            {

            });


            using (HttpClient httpClient = new HttpClient())
            {

                return await httpClient.GetStringAsync(url);
            }
        }
    }
}
