using System.Net;
using System.Net.Http.Headers;
using DataAccess.Entities;
using Generated;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Service.Transactions.Dto;
using Service.Transactions.Enums;
using Service.Transactions.Utils;
using Xunit.Abstractions;
using SystemTransactionRequest = Generated.SystemTransactionRequest;

namespace ApiIntegrationTests;

public class TransactionTests (ITestOutputHelper outputHelper) : ApiTestBase(outputHelper)
{
 
    
    [Fact(DisplayName = "Admin can see all the transactions for all the users")]
    public async Task ViewTransaction_AdminCanView()
    {
        // Arrange
        var client = new Client(TestHttpAdmin);

        // Act
        var response = await client.All3Async();

        // Assert
        Assert.Equal(200, response.StatusCode);
        Assert.NotNull(response.Result);
    }
    
    [Fact(DisplayName = "Users cant see all the transactions for all the users")]
    public async Task ViewTransaction_UsersCantSee()
    {
        // Arrange
        var client = new Client(TestHttpClient);

        // Act
        var exception = await Assert.ThrowsAsync<ApiException>(async () =>
        {
            await client.All3Async();
        });
        
        // Assert
        Assert.Equal(403, exception.StatusCode);
    }
    
    [Fact(DisplayName = "Users can see their own transactions")]
    public async Task ViewTransaction_UserCanSeeTheirOwns()
    {
        // Arrange
        var client = new Client(TestHttpClient);

        // Act
        var response = await client.TransactionAllAsync();
        
        // Assert
        Assert.Equal(200, response.StatusCode);
    }
    
    [Fact(DisplayName = "Unauthenticated user can't see any transaction")]
    public async Task ViewTransaction_UnauthenticatedUserCantSeeAnything()
    {
        // Arrange
        var client = new Client(CreateClient());

        // Act
        var exception = await Assert.ThrowsAsync<ApiException>(async () =>
        {
            await client.TransactionAllAsync();
        });

        // Assert
        Assert.Equal(401, exception.StatusCode);
    }

    [Fact(DisplayName = "Admin can see a specific user transactions")]
    public async Task ViewTransaction_AdminCanSeeUserTransactions()
    {
        // Arrange
        var client = new Client(TestHttpAdmin);
        var user = await UserManager.FindByNameAsync("mockuser1") ?? throw new Exception();
        
        // Act
        var response = await TestHttpAdmin.GetAsync("/api/transaction/user/" + user.Id);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        // Assert.NotNull(response.Result);
    }
    
    [Fact(DisplayName = "A user cannot see other user's transactions")]
    public async Task ViewTransaction_UserCantSeeOtherUserTransactions()
    {
        // Arrange
        var client = new Client(TestHttpClient); // Mockuser1
        var user = await UserManager.FindByNameAsync("mockuser2") ?? throw new Exception();
        
        // Act
        var exception = await Assert.ThrowsAsync<ApiException>(async () =>
        {
            await client.UserAll2Async(Guid.Parse(user.Id));
        });
        // Assert
        Assert.Equal(403, exception.StatusCode);
    }
    
    [Fact(DisplayName = "Admin can send money to users")]
    public async Task CreateTransaction_AdminCanSendMoneyToUsers()
    {
        // Arrange
        var client = new Client(TestHttpAdmin);
        // Proof that user exists in the UserManager
        var user = await UserManager.FindByNameAsync("mockadmin") ?? throw new Exception();
        // Get user id from the logged in jwt 
        var adminJwt = AdminJwt;
        outputHelper.WriteLine(adminJwt); // if you decode it, userId exists in the db users, so i dont' think it's the case
        var dbUser = await PgCtxSetup.DbContextInstance
            .Users
            .Where(u => u.Id == user.Id)
            .FirstOrDefaultAsync() ?? throw new Exception();
        outputHelper.WriteLine("Logged In User:" + dbUser.Id + " - " + dbUser.UserName);

        // More proofs?
        var users = await PgCtxSetup.DbContextInstance
            .Users
            .ToListAsync();
        users.ForEach(u => outputHelper.WriteLine(u.Id  + " - " + u.UserName));
        
        var payload = new SystemTransactionRequest()
        {
            Amount = 20,
            Note = "A note",
            Recipiant = user.Id,
            Operation = SystemOperation.Add
        };
        
        // Act
        var response = await client.SystemAsync(payload); // And yet this is the line that causes the problem !
        
        outputHelper.WriteLine(response.Result.Status);
    
    }
}