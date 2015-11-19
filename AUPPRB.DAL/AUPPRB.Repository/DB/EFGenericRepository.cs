using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using AUPPRB.Models.DomainModels;
using AUPPRB.Models.ViewModels.UsersModels;

namespace AUPPRB.Repository.DB
{
    /// <summary>
    /// EfGenericRepository
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public class EfGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected DbContext DbContext { get; set; }

        protected DbSet<TEntity> DbSet { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext">DbContext</param>
        public EfGenericRepository(DbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("Null DbContext");
            }
            DbContext = dbContext;
            DbSet = DbContext.Set<TEntity>();
        }


        /// <summary>
        /// Returns all objects of given type.
        /// </summary>
        /// <param name="filter">Filter for selecting desired data.</param>
        /// <param name="includeProperties">Included properties.</param>
        /// <returns>List with filtered data.</returns>
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = null)
        {
            IQueryable<TEntity> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return query.ToList();
        }


        /// <summary>
        /// Get collection of elements 
        /// </summary>
        /// <returns>Collection of IQueryable</returns>
        public virtual IQueryable<TEntity> GetAll()
        {
            return DbSet.AsQueryable();
        }

        /// <summary>
        /// Get collection of elements  without caching
        /// </summary>
        /// <returns>Collection of IQueryable</returns>
        public IQueryable<TEntity> GetAllNoTracking()
        {
            return GetAll().AsNoTracking();
        }

        /// <summary>
        /// Get collection of elements which match the predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate).AsQueryable<TEntity>();
        }

        /// <summary>
        /// Get collection of elements which match the predicate whiout caching
        /// </summary>
        /// <param name="predicate">predicate</param>
        /// <returns>Collection of IQuerable</returns>
        public IQueryable<TEntity> FilterNoTracking(Expression<Func<TEntity, bool>> predicate)
        {
            return Filter(predicate).AsNoTracking();
        }

        /// <summary>
        /// Get first element which match predicate
        /// </summary>
        /// <param name="predicate">predicate</param>
        /// <returns>entity</returns>
        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            var tEntity = DbSet.FirstOrDefault(predicate);
            return tEntity;
        }

        /// <summary>
        /// Returns element by id.
        /// </summary>
        /// <param name="id">Entity id</param>
        /// <returns>Entity</returns>
        public virtual TEntity GetById(int id)
        {
            TEntity tEntity = DbSet.Find(id);
            return tEntity;
        }

        /// <summary>
        /// Adds object to repository.
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        /// <summary>
        /// Deletes object from repository by entity.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(TEntity entity)
        {
            if (DbContext.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            DbSet.Remove(entity);

        }


        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(TEntity entity)
        {
            DbEntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
        }


        /// <summary>
        /// Delete entity by id
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(int id)
        {
            TEntity tEntity = GetById(id);
            if (tEntity == null) return;
            Delete(tEntity);
        }

        /// <summary>
        /// Delete range of entities
        /// </summary>
        /// <param name="items"></param>
        public void DeleteAll(IEnumerable<TEntity> items)
        {
            DbSet.RemoveRange(items);
        }

    }
}
