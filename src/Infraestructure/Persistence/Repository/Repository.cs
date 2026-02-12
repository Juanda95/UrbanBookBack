using Microsoft.EntityFrameworkCore;
using Persistence.Repository.Interface;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Persistence.Repository
{
    /// <summary>
    /// Inicializa una nueva instancia de la clase Repository con el DbContext especificado.
    /// </summary>
    /// <param name="dbcontext">El DbContext utilizado para interactuar con la base de datos.</param>
    public class Repository<TEntity>(DbContext dbcontext) : IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _entities = dbcontext.Set<TEntity>();

        /// <summary>
        /// Aplica las inclusiones especificadas a una consulta IQueryable<TEntity>.
        /// </summary>
        /// <param name="includeProperties">Las propiedades de navegación que se deben incluir en la consulta.</param>
        /// <param name="query">La consulta IQueryable<TEntity> a la que se aplicarán las inclusiones.</param>
        /// <returns>Una consulta IQueryable<TEntity> con las inclusiones aplicadas.</returns>
        private IQueryable<TEntity> PerformInclusions(IEnumerable<Expression<Func<TEntity, object>>> includeProperties,
                                               IQueryable<TEntity> query)
        {
            return includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        private IQueryable<TEntity> PerformInclusions(IEnumerable<Func<IQueryable<TEntity>, IQueryable<TEntity>>> includeActions,
                                                      IQueryable<TEntity> query)
        {
            foreach (var action in includeActions)
            {
                query = action(query);
            }

            return query;
        }


        /// <summary>
        /// Realiza una consulta LINQ sobre las entidades de tipo TEntity en la base de datos.
        /// </summary>
        public IQueryable<TEntity> AsQueryable()
        {
            return _entities.AsQueryable<TEntity>();
        }

        /// <summary>
        /// Obtiene todas las entidades de tipo TEntity de la base de datos, incluyendo propiedades relacionadas según sea necesario.
        /// </summary>
        /// <param name="includeProperties">Las propiedades de navegación que se deben incluir en la consulta.</param>
        /// <returns>Una colección de entidades de tipo TEntity.</returns>
        public IEnumerable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = AsQueryable();
            return PerformInclusions(includeProperties, query);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {

            IQueryable<TEntity> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return await query.ToListAsync();

        }

        public async Task<IEnumerable<TEntity>> GetAllAsyncThenInclude(params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includeActions)
        {
            IQueryable<TEntity> query = AsQueryable();
            query = PerformInclusions(includeActions, query);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsyncThenIncludeWhere(Expression<Func<TEntity, bool>> where, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includeActions)
        {
            IQueryable<TEntity> query = AsQueryable();
            query = PerformInclusions(includeActions, query);
            return await query.Where(where).ToListAsync();
        }

        /// <summary>
        /// Busca una entidad de tipo TEntity que coincida con el predicado especificado.
        /// </summary>
        /// <param name="predicate">El predicado utilizado para buscar la entidad.</param>
        /// <returns>La primera entidad que coincida con el predicado, o null si no se encuentra ninguna entidad.</returns>
        public TEntity? Find(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = AsQueryable();
            return query.FirstOrDefault(predicate);
        }

        /// <summary>
        /// Obtiene una colección de entidades de tipo TEntity que coincidan con el predicado especificado, incluyendo propiedades relacionadas según sea necesario.
        /// </summary>
        /// <param name="where">El predicado utilizado para filtrar las entidades.</param>
        /// <param name="includeProperties">Las propiedades de navegación que se deben incluir en la consulta.</param>
        /// <returns>Una colección de entidades de tipo TEntity que coinciden con el predicado.</returns>
        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return query.Where(where);
        }


        /// </summary>
        /// <param name="where">El predicado utilizado para filtrar las entidades.</param>
        /// <param name="includeProperties">Las propiedades de navegación que se deben incluir en la consulta.</param>
        /// <param name="stoppingToken">Token de cancelación para la operación asincrónica.</param>
        /// <returns>Una colección de entidades de tipo TEntity que coinciden con el predicado.</returns>
        public Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return query.Where(where).ToListAsync();
        }

        /// <summary>
        /// Obtiene una colección de entidades de tipo TEntity que coincidan con el predicado especificado, sin rastrear en el contexto de la base de datos.
        /// </summary>
        /// <param name="where">El predicado utilizado para filtrar las entidades.</param>
        /// <param name="includeProperties">Las propiedades de navegación que se deben incluir en la consulta.</param>
        /// <returns>Una colección de entidades de tipo TEntity que coinciden con el predicado.</returns>
        public IEnumerable<TEntity> FindAllNoTracking(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = AsQueryable().AsNoTracking();
            query = PerformInclusions(includeProperties, query);
            return query.Where(where);
        }

        /// <summary>
        /// Obtiene la primera entidad de tipo TEntity que coincide con el predicado especificado, incluyendo propiedades relacionadas según sea necesario.
        /// </summary>
        /// <param name="where">El predicado utilizado para filtrar las entidades.</param>
        /// <param name="includeProperties">Las propiedades de navegación que se deben incluir en la consulta.</param>
        /// <returns>La primera entidad que coincide con el predicado.</returns>
        public TEntity First(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return query.First(where);
        }

        /// <summary>
        /// Obtiene la última entidad de tipo TEntity que coincide con el predicado especificado, incluyendo propiedades relacionadas según sea necesario.
        /// </summary>
        /// <param name="where">El predicado utilizado para filtrar las entidades.</param>
        /// <param name="includeProperties">Las propiedades de navegación que se deben incluir en la consulta.</param>
        /// <returns>La última entidad que coincide con el predicado.</returns>
        public TEntity Last(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return query.Last(where);
        }

        /// <summary>
        /// Obtiene la primera entidad de tipo TEntity que coincide con el predicado especificado, o null si no se encuentra ninguna entidad.
        /// </summary>
        /// <param name="where">El predicado utilizado para filtrar las entidades.</param>
        /// <param name="includeProperties">Las propiedades de navegación que se deben incluir en la consulta.</param>
        /// <returns>La primera entidad que coincide con el predicado, o null si no se encuentra ninguna entidad.</returns>
        public TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return query.FirstOrDefault(where);
        }

        /// <summary>
        /// Obtiene la primera entidad de tipo TEntity que coincide con el predicado especificado, o null si no se encuentra ninguna entidad.
        /// </summary>
        /// <param name="where">El predicado utilizado para filtrar las entidades.</param>
        /// <param name="includeProperties">Las propiedades de navegación que se deben incluir en la consulta.</param>
        /// <returns>La primera entidad que coincide con el predicado, o null si no se encuentra ninguna entidad.</returns>
        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return await query.FirstOrDefaultAsync(where);
        }

        public async Task<TEntity?> FirstOrDefaultAsyncThenInclude(Expression<Func<TEntity, bool>> where, params Func<IQueryable<TEntity>, IQueryable<TEntity>>[] includeActions)
        {
            IQueryable<TEntity> query = AsQueryable();
            query = PerformInclusions(includeActions, query);
            return await query.FirstOrDefaultAsync(where);
        }

        /// <summary>
        /// Obtiene la primera entidad de tipo TEntity que coincide con el predicado especificado, sin rastrear en el contexto de la base de datos.
        /// </summary>
        /// <param name="where">El predicado utilizado para filtrar las entidades.</param>
        /// <param name="includeProperties">Las propiedades de navegación que se deben incluir en la consulta.</param>
        /// <returns>La primera entidad que coincide con el predicado.</returns>
        public TEntity? FirstOrDefaultNotTracking(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return query.AsNoTracking().FirstOrDefault(where);
        }

        /// <summary>
        /// Obtiene la última entidad de tipo TEntity que coincide con el predicado especificado, o null si no se encuentra ninguna entidad.
        /// </summary>
        /// <param name="where">El predicado utilizado para filtrar las entidades.</param>
        /// <param name="includeProperties">Las propiedades de navegación que se deben incluir en la consulta.</param>
        /// <returns>La última entidad que coincide con el predicado, o null si no se encuentra ninguna entidad.</returns>
        public TEntity? LastOrDefault(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return query.LastOrDefault(where);
        }

        /// <summary>
        /// Obtiene una sola entidad de tipo TEntity que coincide con el predicado especificado.
        /// </summary>
        /// <param name="where">El predicado utilizado para filtrar las entidades.</param>
        /// <param name="includeProperties">Las propiedades de navegación que se deben incluir en la consulta.</param>
        /// <returns>La única entidad que coincide con el predicado.</returns>
        public TEntity Single(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return query.Single(where);
        }

        /// <summary>
        /// Obtiene una sola entidad de tipo TEntity que coincide con el predicado especificado, o null si no se encuentra ninguna entidad.
        /// </summary>
        /// <param name="where">El predicado utilizado para filtrar las entidades.</param>
        /// <param name="includeProperties">Las propiedades de navegación que se deben incluir en la consulta.</param>
        /// <returns>La única entidad que coincide con el predicado, o null si no se encuentra ninguna entidad.</returns>
        public TEntity? SingleOrDefault(Expression<Func<TEntity, bool>> where, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = AsQueryable();
            query = PerformInclusions(includeProperties, query);
            return query.SingleOrDefault(where);
        }

        /// <summary>
        /// Inserta una nueva entidad de tipo TEntity en la base de datos.
        /// </summary>
        /// <param name="entity">La entidad que se va a insertar.</param>
        public void Insert(TEntity entity)
        {
            _entities.Add(entity);
        }

        /// <summary>
        /// Inserta varias entidades de tipo TEntity en la base de datos.
        /// </summary>
        /// <param name="entities">Las entidades que se van a insertar.</param>
        public void Insert(IEnumerable<TEntity> entities)
        {
            dbcontext.Set<TEntity>().AddRange(entities);
        }

        /// <summary>
        /// Actualiza una entidad de tipo TEntity en la base de datos.
        /// </summary>
        /// <param name="entity">La entidad que se va a actualizar.</param>
        public void Update(TEntity entity)
        {
            _entities.Attach(entity);
            dbcontext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Actualiza varias entidades de tipo TEntity en la base de datos.
        /// </summary>
        /// <param name="entities">Las entidades que se van a actualizar.</param>
        public void Update(IEnumerable<TEntity> entities)
        {
            foreach (var e in entities)
            {
                dbcontext.Entry(e).State = EntityState.Modified;
            }
        }

        /// <summary>
        /// Elimina una entidad de tipo TEntity de la base de datos.
        /// </summary>
        /// <param name="entity">La entidad que se va a eliminar.</param>
        public void Delete(TEntity entity)
        {
            if (dbcontext.Entry(entity).State == EntityState.Detached)
            {
                _entities.Attach(entity);
            }
            _entities.Remove(entity);
        }

        /// <summary>
        /// Elimina varias entidades de tipo TEntity de la base de datos.
        /// </summary>
        /// <param name="entities">Las entidades que se van a eliminar.</param>
        public void Delete(IEnumerable<TEntity> entities)
        {
            foreach (var e in entities)
            {
                dbcontext.Entry(e).State = EntityState.Deleted;
            }
        }


    }
}
