using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using My.FrameworkCore.Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace My.FrameworkCore.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class MallDbContext : DbContext
    {
        public MallDbContext(DbContextOptions<MallDbContext> options) : base(options)
        {

        }

        //自定义DbContext实体属性名与数据库表对应名称（默认 表名与属性名对应是 User与Users）
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>().ToTable("Users");
        }

        public DbSet<User> Users { get; set; }

    }
}
