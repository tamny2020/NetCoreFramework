using My.FrameworkCore.Models.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using My.FrameworkCore.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace My.FrameworkCore.Services.Implement
{

    public class UserService : EFRepository<User>, IUserService, IService
    {

        public UserService(MallDbContext context) : base(context)
        {

        }

    }
}
