using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AUPPRB.Repository.DB
{
    /// <summary>
    /// Mediator interface that acts as an abstraction layer between domain and data layer.
    /// Maintains a list of domain model objects of given type.
    /// Provides CRUD (create, read, update, delete) operations on given type of objects.
    /// </summary>
  public  interface IGenericRepository<T> where T:class 
    {
        /// <summary>
        /// Returns all objects of given type.
        /// </summary>
        IEnumerable<T> Get(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = "");


        IQueryable<T> GetAll();

        IQueryable<T> GetAllNoTracking();

        IQueryable<T> Filter(Expression<Func<T, bool>> predicate);

        IQueryable<T> FilterNoTracking(Expression<Func<T, bool>> predicate);

        T FirstOrDefault(Expression<Func<T, bool>> predicate);


      /// <summary>
      /// Returns element by id
      /// </summary>
      /// <param name="id"></param>
      /// <returns></returns>
        T GetById(int id);

      /// <summary>
      /// Adds object to repository.
      /// </summary>
      /// <param name="entity"></param>
        void Add(T entity);

      /// <summary>
      /// Updates object in repository.
      /// </summary>
      /// <param name="entity"></param>
        void Update(T entity);

      /// <summary>
      /// Deletes object from repository by entity.
      /// </summary>
      /// <param name="entity"></param>
        void Delete(T entity);

      /// <summary>
      /// Deletes object from repository by id.
      /// </summary>
      /// <param name="id"></param>
        void Delete(int id);

      /// <summary>
      /// Delete range of entities
      /// </summary>
      /// <param name="items"></param>
        void DeleteAll(IEnumerable<T> items);


    }
}
