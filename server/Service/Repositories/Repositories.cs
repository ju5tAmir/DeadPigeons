using DataAccess;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Service.Repositories;

public class UserRepository(AppDbContext context) : BaseRepository<User>(context)
{
    protected override DbSet<User> Set => Context.Users;
}

public class PreferenceRepository(AppDbContext context) : BaseRepository<DataAccess.Entities.Preference>(context)
{
    protected override DbSet<DataAccess.Entities.Preference> Set => Context.Preferences;
}

public abstract class BaseRepository<T>(AppDbContext context) : IRepository<T> where T : class
{
    protected AppDbContext Context => context;
    protected abstract DbSet<T> Set { get; }

    public async Task Add(T entity)
    {
        await Set.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        Set.Remove(entity);
        await context.SaveChangesAsync();
    }

    public T? Get(Func<T, bool> predicate)
    {
        return Set.Where(predicate).SingleOrDefault();
    }

    public IQueryable<T> Query()
    {
        return Set;
    }

    public async Task Update(T entity)
    {
        Set.Update(entity);
        await context.SaveChangesAsync();
    }
}