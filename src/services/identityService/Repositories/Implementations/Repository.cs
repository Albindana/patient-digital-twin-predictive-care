using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using IdentityService.Data;
using IdentityService.Entities;
using IdentityService.Repositories.Interfaces;

namespace IdentityService.Repositories.Implementations;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _context;

    public Repository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();
    public async Task<T?> GetByIdAsync(int id) => await _context.Set<T>().FindAsync(id);
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) 
        => await _context.Set<T>().Where(predicate).ToListAsync();
    public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);
    public void Update(T entity) => _context.Set<T>().Update(entity);
    public void Delete(T entity) => _context.Set<T>().Remove(entity);
    public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
}