using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace My.FrameworkCore.IdentityServiceAPI
{
    public class IdentityServerConfig
    {
        /// <summary>
        /// 注册ApiResource，即授权后可访问的Api（PS：ApiResource对应的是OAuth2.0中的Scope）
        /// 表示IdentityServer管理的所有的下游服务列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("UserAPI", "UserAPI"){
                        Description ="用户接口",
                        UserClaims = { "role", JwtClaimTypes.Role
                    }
                },
                new ApiResource("OrderAPI", "OrderAPI"){
                        Description ="订单接口",
                        UserClaims = { "role", JwtClaimTypes.Role
                    }
                }
            };
        }
        /// <summary>
        /// 注册IdentityResource，即授权后客户端可访问的用户信息（PS：IdentityResource对应的是OpenId Connect中的Scope）
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone()
            };
        }
        /// <summary>
        /// 客户端注册，定义给客户端可以返回的资源，即允许哪个Scope定义
        /// 表示IdentityServer管理的所有的上游客户端列表
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {

            /*
            http://www.jessetalk.cn/2018/04/04/oidc-asp-net-core/
            我们简单的来理解一下这三种模式：
            Authorization Code Flow授权码模式：保留oAuth2下的授权模式不变response_type = code

            Implicit Flow 隐式模式：在oAuth2下也有这个模式，主要用于客户端直接可以向授权服务器获取token，跳过中间获取code用code换accesstoken的这一步。在OIDC下，responsetype = token idtoken，也就是可以同时返回access_token和id_token。

            Hybrid Flow 混合模式： 比较有典型的地方是从authorize endpoint 获取 code idtoken，这个时候id_token可以当成认证。而可以继续用code获取access_token去做授权，比隐式模式更安全。再来详细看一下这三种模式的差异：
            */

            return new List<Client>
            {
                new Client
                {
                    ClientId = "postman", //客户端ID
                    //三种模式: Code，Implict和Hybird。
                    //因为这三种模式决定了我们的response_type可以请求哪几个值，所以这个地方一定不能写错
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    //AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,//认证模式

                    RedirectUris = { "https://localhost:5001/oauth2/callback" },

                    //是否出现授权同意页面
                    //RequireConsent=false,

                    //客户端对应的密钥
                    ClientSecrets ={ new Secret("secret".Sha256()) },

                    //该客户端支持访问的下游服务列表，必须是在 ApiResources列表中登记的
                    AllowedScopes = new List<string>
                    {
                        "UserAPI", //允许访问的api资源，即在上面方法中指定的api
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    },
                    //AllowOfflineAccess=true,
                },
                new Client
                {
                    ClientId = "orderApi", //客户端ID
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    RedirectUris = { "https://localhost:5001/oauth2/callback" },
                    ClientSecrets ={ new Secret("123456".Sha256()) },

                    //RequireClientSecret=false,//禁用客户端密码

                    AllowedScopes = new List<string>
                    {
                        "OrderAPI", //允许访问的api资源，即在上面方法中指定的api
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    },
                },

            };
        }
        /// <summary>
        /// 注册用户，这里就用IdentityServer4提供的TestUser进行测试
        /// </summary>
        /// <returns></returns>
        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser> {
            new TestUser {
                SubjectId = "100",
                Username = "tamny",
                Password = "123456",
                Claims = new List<Claim> {
                    new Claim(JwtClaimTypes.Name, "tamny"),
                    new Claim(JwtClaimTypes.FamilyName, "tang"),
                    new Claim(JwtClaimTypes.Email, "tamny2019@qq.com"),
                    new Claim(JwtClaimTypes.Role, "admin"),
                }
            }
            };
        }

    }
}
