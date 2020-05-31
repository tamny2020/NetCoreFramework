using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace My.FrameworkCore.UserAPI.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        //[Authorize]
        [Route("GetName")]
        public string GetName()
        {
            //consul 服务发现
            /*
            using (var consulClient = new ConsulClient(a => a.Address = new Uri("http://localhost:8500")))
            {
                var services = consulClient.Catalog.Service("UserAPI").Result.Response;
                //consulClient.Agent.Services();
            }*/
            return "李晓明";
        }

        [HttpGet]
        [Route("GetClaims")]
        public IActionResult GetClaims()
        {
            var list = (from c in HttpContext.User.Claims
                        select new
                        {
                            c.Type,
                            c.Value
                        }).ToList();

            return new JsonResult(list);
        }


        // GET api/values/5
        [HttpGet()]
        [Route("GetUser")]
        public string GetUser()
        {
            return "王小贡";
        }

        [Route("Health")]
        [HttpGet]
        public IActionResult Health()
        {
            return Ok();
        }

        // POST api/values
        [Route("Post")]
        [HttpPost]
        public void Post([FromBody]string value)
        {

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
