using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace My.FrameworkCore.IdentityServiceAPI
{
    /// <summary>
    /// IdentityServer提供了接口访问用户信息，但是默认返回的数据只有sub，就是上面设置的subject: context.UserName，要返回更多的信息，需要实现IProfileService接口：
    /// </summary>
    public class MyProfileService : IProfileService
    {

        public ApplicationSessionUser _applicationSessionUser;

        public MyProfileService(ApplicationSessionUser applicationSessionUser)
        {
            _applicationSessionUser = applicationSessionUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Claim>> GetClaimsFromUserAsync()
        {
            return new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject,"用户Id"),
                new Claim(JwtClaimTypes.PreferredUserName,"用户姓名"),
                new Claim(JwtClaimTypes.Role,"用户角色"),
                new Claim("img","用户头像"),
            };
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                //context.Subject.Claims就是之前实现IResourceOwnerPasswordValidator接口时claims: GetUserClaims()给到的数据。

                //var subjectId = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub").Value;
                //var user = GetUserById(subjectId);
                var claims = await GetClaimsFromUserAsync();
                context.IssuedClaims = claims;
            }
            catch (Exception ex)
            {
                //log your error
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = false;

            var subjectId = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub").Value;

            //从数据库中查询用户进行验证
            if (subjectId == _applicationSessionUser.Id.ToString())
            {
                context.IsActive = true;
            }
        }
    }
}
