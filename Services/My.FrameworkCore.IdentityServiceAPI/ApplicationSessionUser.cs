using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace My.FrameworkCore.IdentityServiceAPI
{
    public class ApplicationSessionUser : IdentityUser<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public override int Id { set; get; }
        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { set; get; }
    }
}
