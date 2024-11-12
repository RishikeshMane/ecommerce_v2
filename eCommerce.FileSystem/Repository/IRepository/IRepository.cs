﻿using System.Linq.Expressions;

namespace FileSystem.Repository.IRepository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        /// TEntity - Product[TableName]
        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter, string? includeProperties = null);
        IEnumerable<TEntity> GetAll(string? includeProperties = null);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter, string? includeProperties = null);
        void Add(TEntity entity);
        void AddAsync(TEntity entity);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
