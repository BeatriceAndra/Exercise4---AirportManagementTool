using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Repositories
{
    public class Repository<T> where T : class
    {
        protected readonly AirportManagementContext _context;
        protected readonly IMapper _mapper;
        protected readonly DbSet<T> _dbSet;

        public Repository(AirportManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _dbSet = _context.Set<T>();
        }

        public Repository(AirportManagementContext context)
        {
            _context = context;
        }

        // --- Get by Id ---
        public virtual async Task<T> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Entity of type {typeof(T).Name} with Id {id} not found.");
            return entity;
        }

        public virtual async Task<TResult> GetByIdAsync<TResult>(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Entity of type {typeof(T).Name} with Id {id} not found.");
            return _mapper.Map<TResult>(entity);
        }

        // --- Get all ---
        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<List<TResult>> GetAllAsync<TResult>()
        {
            return await _dbSet.ProjectTo<TResult>(_mapper.ConfigurationProvider).ToListAsync();
        }

        // --- Add ---
        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<TResult> AddAsync<TSource, TResult>(TSource source)
        {
            var entity = _mapper.Map<T>(source);
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<TResult>(entity);
        }

        // --- Update ---
        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync<TSource>(TSource source, int id)
        {
            var entity = await GetByIdAsync(id);
            _mapper.Map(source, entity);
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        // --- Delete ---
        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        // --- Exists ---
        public virtual async Task<bool> ExistsAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity != null;
        }

        // --- Find by predicate ---
        public virtual async Task<List<T>> FindAsync(System.Linq.Expressions.Expression<System.Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<List<TResult>> FindAsync<TResult>(System.Linq.Expressions.Expression<System.Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ProjectTo<TResult>(_mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
