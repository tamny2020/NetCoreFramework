using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.FrameworkCore.Services
{
    /// <summary>
    /// 接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 
        /// </summary>
        DbContext DbContext { get; }

        /// <summary>
        /// 
        /// </summary>
        DbSet<TEntity> Entities { get; }

        /// <summary>
        /// 
        /// </summary>
        IQueryable<TEntity> Table { get; }

        Task<int> Insert(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        TEntity GetById(int id);

    }
}
