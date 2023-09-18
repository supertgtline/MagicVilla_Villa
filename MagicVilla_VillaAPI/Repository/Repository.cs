using System.Linq.Expressions;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Repository
{

    public class Repository<T> : IRepository<T> where T:class
    {
        private readonly ApplicationDbContext _db;
        private DbSet<T> _dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
          //  _db.VillaNumbers.Include(u => u.Villa).ToList();
            this._dbSet = _db.Set<T>();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null,string? includeProperties = null, int pageSize = 0, int pageNumber = 1)
        {
            IQueryable<T> queryable = _dbSet;
            
            if ((filter != null))
            {
                queryable = queryable.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(includeProp);
                }
            }
            if (pageSize > 0)
            {
                if (pageSize > 100)
                {
                    pageSize = 100;
                }

                queryable = queryable.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }

            return await queryable.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<T> queryable = _dbSet;
            if (!tracked)
            {
                queryable = queryable.AsNoTracking();
            }

            if (filter != null)
            {
                queryable = queryable.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(includeProp);
                }
            }
            return await queryable.FirstOrDefaultAsync();
        }

        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveAsync();
        }
        
        public async Task RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}