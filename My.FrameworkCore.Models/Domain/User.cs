using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;


namespace My.FrameworkCore.Models.Domain
{

    [Table("sys_User")]
    public class User
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Column("id")]
        public int Id { set; get; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { set; get; }
    }
}
