using AppointmentSystemMicroservice.DAL;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppointmentSystemMicroservice.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly IGenericRepository<TEntity> _genericRepository;
    private readonly AppDbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _dbContext = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            _dbContext.Entry(entity).State = EntityState.Detached;
        }
        return entity;
    }

    public void Remove(TEntity entity)
    {
        _dbContext.Remove(entity);
        _dbContext.SaveChanges();
    }

    public TEntity Update(TEntity entity)
    {
        _dbContext.Update(entity);
        _dbContext.SaveChanges();
        return entity;
    }

    public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.Where(predicate);
    }
}
