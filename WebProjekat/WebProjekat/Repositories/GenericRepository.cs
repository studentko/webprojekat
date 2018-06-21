using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using WebProjekat.Models;

namespace WebProjekat.Repositories.UnitOfWork
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal AppDbContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository() { }

        public GenericRepository(AppDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetByID(int id)
        {

            return dbSet.Find(id);

        }

        public virtual TEntity Insert(TEntity entity)
        {
            var returnEntity = dbSet.Add(entity);
            Save();
            return returnEntity;
        }

        public virtual TEntity Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
            return entityToDelete;
        }

        public virtual TEntity Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
            Save();
            return entityToDelete;
        }

        public virtual TEntity Update(TEntity entityToUpdate)
        {
            dbSet.AddOrUpdate(entityToUpdate);
            Save();
            return entityToUpdate;
        }

        private void Save()
        {
            context.SaveChanges();
        }
    }
}