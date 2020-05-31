using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.FrameworkCore.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EFRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private MallDbContext _context;

        public EFRepository(MallDbContext context)
        {
            this._context = context;
        }
        public DbContext DbContext { get { return _context; } }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<TEntity> Entities { get { return _context.Set<TEntity>(); } }
        /// <summary>
        /// 
        /// </summary>
        public IQueryable<TEntity> Table { get { return Entities; } }

        public async Task<int> Insert(TEntity entity)
        {
            await Entities.AddAsync(entity);
            return await _context.SaveChangesAsync();
        }

        public void Update(TEntity user)
        {
            _context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            Entities.Remove(entity);
            _context.SaveChanges();
        }

        public TEntity GetById(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }

    }
}
