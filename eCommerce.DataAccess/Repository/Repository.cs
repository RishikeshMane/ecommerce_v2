using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using eCommerce.Data;
using eCommerce.DataAccess.Repository;
using eCommerce.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly eCommerceDbContext _db;
        internal DbSet<TEntity> _dbSet;

        public Repository(eCommerceDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();
            ///_db.Products.Include(u => u.Category).Include(u => u.CoverType);
        }
        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void AddAsync(TEntity entity)
        {
            _dbSet.AddAsync(entity);
        }

        public IEnumerable<TEntity> GetAll(string? includeProperties = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.ToList();
        }

        ///includeProperties = "Category,CoverType"

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter, string? includeProperties = null)
        {
            IQueryable<TEntity> query = _dbSet;
            query = query.Where(filter);

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.ToList();
        }

        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter, string? includeProperties = null)
        {
            IQueryable<TEntity> query = _dbSet;
            query = query.Where(filter);

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.FirstOrDefault();
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
