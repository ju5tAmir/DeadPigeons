using DataAccess;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Service.Repositories;

public class UserRepository(AppDbContext context) : BaseRepository<User>(context)
{
    protected override DbSet<User> Set => Context.Users;
}

public class GameRepository(AppDbContext context) : BaseRepository<DataAccess.Entities.Game>(context)
{
    protected override DbSet<DataAccess.Entities.Game> Set => Context.Games;
}
public class BoardRepository(AppDbContext context) : BaseRepository<DataAccess.Entities.Board>(context)
{
    protected override DbSet<DataAccess.Entities.Board> Set => Context.Boards;
}

public class PackageRepository(AppDbContext context) : BaseRepository<DataAccess.Entities.Package>(context)
{
    protected override DbSet<DataAccess.Entities.Package> Set => Context.Packages;
}

public class WinnerRepository(AppDbContext context) : BaseRepository<DataAccess.Entities.Winner>(context)
{
    protected override DbSet<DataAccess.Entities.Winner> Set => Context.Winners;
}

public class TransactionRepository(AppDbContext context) : BaseRepository<DataAccess.Entities.Transaction>(context)
{
    protected override DbSet<DataAccess.Entities.Transaction> Set => Context.Transactions;
}

public class ManualPaymentRepository(AppDbContext context) : BaseRepository<DataAccess.Entities.ManualPayment>(context)
{
    protected override DbSet<DataAccess.Entities.ManualPayment> Set => Context.ManualPayments;
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