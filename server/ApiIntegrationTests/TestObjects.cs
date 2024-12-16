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
}