using System.Runtime.InteropServices.JavaScript;
using Bogus;
using DataAccess.Entities;

namespace ApiIntegrationTests;

public class TestObjects
{
    public static User GetUser()
    {
        return new Faker<User>()
            .RuleFor(p => p.Id, f => Guid.NewGuid().ToString())
            .RuleFor(p => p.FirstName, f => f.Name.FirstName())
            .RuleFor(p => p.LastName, f => f.Name.LastName())
            .RuleFor(p => p.Balance, 0)
            .RuleFor(p => p.Email, f => f.Internet.Email())
            .RuleFor(p => p.PhoneNumber, "123456789")
            .RuleFor(p => p.IsActive, true)
            .RuleFor(p => p.IsAutoPlay, false)
            .RuleFor(p => p.RegistrationDate, f => DateTime.UtcNow);
    }
    
    public static Game GetGame()
    {
        return new Faker<Game>()
            .RuleFor(p => p.GameId, f => Guid.NewGuid())
            .RuleFor(p => p.Year, 2024)
            .RuleFor(p => p.WeekNumber, 50)
            .RuleFor(p => p.OnlinePlayers, f => f.Random.Int(1, 100))
            .RuleFor(p => p.OnlineWinningPlayers, f => f.Random.Int(1, 5))
            .RuleFor(p => p.OnlineBoards, f => f.Random.Int(1, 300))
            .RuleFor(p => p.OnlineWinningBoards, f => f.Random.Int(5, 20))
            .RuleFor(p => p.OnlineIncome, f => f.Random.Int(500, 2000))
            .RuleFor(p => p.OnlinePayout, f => f.Random.Int(200, 1000))
            .RuleFor(p => p.OfflinePlayers, f => f.Random.Int(1, 100))
            .RuleFor(p => p.OfflineWinningPlayers, f => f.Random.Int(1, 5))
            .RuleFor(p => p.OfflineBoards, f => f.Random.Int(1, 300))
            .RuleFor(p => p.OfflineWinningBoards, f => f.Random.Int(5, 20))
            .RuleFor(p => p.OfflineIncome, f => f.Random.Int(500, 2000))
            .RuleFor(p => p.OfflinePayout, f => f.Random.Int(200, 1000));
    }
}