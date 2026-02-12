using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Persistence.Repository.Interface
{
    public interface IRepository<TEntity> where TEntity : class
    {   

        /// <summary>
        /// Retorna un objeto IQueryable del tipo TEntity para realizar consultas.
        /// </summary>
        IQueryable<TEntity> AsQueryable();

        /// <summary>
        /// Retorna todas las entidades del tipo TEntity y acepta relaciones a incluir como parámetros.
        /// </summary>
        IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Retorna todas las entidades del tipo TEntity y acepta relaciones a incluir como parámetros.
        /// </summary>
        Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> GetAllAsyncThenInclude(params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includeActions);

        Task<IEnumerable<TEntity>> GetAllAsyncThenIncludeWhere(Expression<Func<TEntity, bool>> where, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includeActions);

        /// <summary>
        /// Encuentra y retorna una entidad del tipo TEntity que coincida con el predicado especificado.
        /// </summary>
        TEntity? Find(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Encuentra y retorna todas las entidades del tipo TEntity que coincidan con el predicado especificado.
        /// </summary>
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Encuentra y retorna todas las entidades del tipo TEntity que coincidan con el predicado especificado.
        /// </summary>
        Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Retorna la última entidad del tipo TEntity que coincida con el predicado especificado.
        /// </summary>
        TEntity First(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Retorna la primera entidad del tipo TEntity que coincida con el predicado especificado.
        /// </summary>
        TEntity Last(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Retorna la primera entidad del tipo TEntity que coincida con el predicado especificado, o null si no se encontraron registros.
        /// </summary>
        TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);
      
        /// <summary>
        /// Retorna la primera entidad del tipo TEntity que coincida con el predicado especificado, o null si no se encontraron registros.
        /// </summary>
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);

        Task<TEntity?> FirstOrDefaultAsyncThenInclude(Expression<Func<TEntity, bool>> where, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includeActions);

        /// <summary>
        /// Retorna la primera entidad del tipo TEntity que coincida con el predicado especificado, sin rastreo de entidades.
        /// </summary>
        TEntity? FirstOrDefaultNotTracking(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Retorna la última entidad del tipo TEntity que coincida con el predicado especificado, o null si no se encontraron registros.
        /// </summary>
        TEntity? LastOrDefault(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Retorna una entidad del tipo TEntity que coincida con el predicado especificado.
        /// </summary>
        TEntity Single(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Retorna una entidad del tipo TEntity que coincida con el predicado especificado, o null si no se encontraron registros.
        /// </summary>
        TEntity? SingleOrDefault(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Inserta una nueva entidad del tipo TEntity.
        /// </summary>
        void Insert(TEntity entity);

        /// <summary>
        /// Inserta varias entidades del tipo TEntity.
        /// </summary>
        void Insert(IEnumerable<TEntity> entities);

        /// <summary>
        /// Actualiza una entidad del tipo TEntity.
        /// </summary>
        void Update(TEntity entity);
                   
        /// <summary>
        /// Actualiza varias entidades del tipo TEntity.
        /// </summary>
        void Update(IEnumerable<TEntity> entities);

        /// <summary>
        /// Elimina una entidad del tipo TEntity.
        /// </summary>
        void Delete(TEntity entity);

        /// <summary>
        /// Elimina un conjunto de entidades del tipo TEntity.
        /// </summary>
        void Delete(IEnumerable<TEntity> entities);

    }
}
