using Generated;
using Service.Game;
using Service.Utils;
using Xunit.Abstractions;

namespace ApiIntegrationTests;

public class GameTests (ITestOutputHelper outputHelper) : ApiTestBase(outputHelper)
{

    
    [Fact(DisplayName = "Admin can start a game")]
    public async Task GameStart_AdminCanStartGame()
    {
        // Arrange
        var adminClient = new Client(TestHttpAdmin);

        var payload = new StartGameRequest()
        {
            Week = TimeUtils.GetCurrentWeekNumber(),
            Year = TimeUtils.GetCurrentYear()
        };
        
        // Act
        var response = await adminClient.StartAsync(payload);

        // Assert
        Assert.Equal(200, response.StatusCode);
        Assert.NotNull(response.Result.GameId.ToString());
    }
    
    [Fact(DisplayName = "A regular user cannot start a game")]
    public async Task GameStart_RegularUserCannotStartGame()
    {
        // Arrange
        var client = new Client(TestHttpClient);

        var payload = new StartGameRequest()
        {
            Week = TimeUtils.GetCurrentWeekNumber(),
            Year = TimeUtils.GetCurrentYear()
        };
        
        // Act
        var exception = await Assert.ThrowsAsync<ApiException>(async () =>
        {
            await client.StartAsync(payload);
        });
        
        // Assert
        Assert.Equal(403, exception.StatusCode);
    }
    
    [Fact(DisplayName = "A game cannot be started in the past")]
    public async Task GameStart_GameCannotStartedInPast()
    {
        // Arrange
        var client = new Client(TestHttpAdmin);

        var payload = new StartGameRequest()
        {
            Week = TimeUtils.GetCurrentWeekNumber(),
            Year = TimeUtils.GetCurrentYear() - 1
        };
        
        // Act
        var exception = await Assert.ThrowsAsync<ApiException>(async () =>
        {
            await client.StartAsync(payload);
        });
        
        // Verify the exception details
        Assert.Equal(500, exception.StatusCode);
        Assert.Contains("A game cannot be started in the past", exception.Response);
    }
    
    [Fact(DisplayName = "Admin can finish a game with proper winning numbers")]
    public async Task GameFinish_AdminCanFinishGame()
    {
        // Arrange
        var client = new Client(TestHttpAdmin);
        var game = TestObjects.GetGame();
        
        await PgCtxSetup.DbContextInstance.Games.AddAsync(game);
        await PgCtxSetup.DbContextInstance.SaveChangesAsync();
        
        var payload = new FinishGameRequest()
        {
            GameId = game.GameId,
            WinningSequence = [1,2,3]
        };
        
        // Act
        var response = await client.FinishAsync(payload);
        
        // Assert
        Assert.Equal(GameStatus.Finished, response.Result.Status);
    }
    
    
    [Fact(DisplayName = "Admin cannot finish games with numbers out of range from 1 to 16")]
    public async Task GameFinish_AdminCannotFinishGameWithBadNumbers()
    {
        // Arrange
        var client = new Client(TestHttpAdmin);
        var game = TestObjects.GetGame();
        
        await PgCtxSetup.DbContextInstance.Games.AddAsync(game);
        await PgCtxSetup.DbContextInstance.SaveChangesAsync();
        
        var payload = new FinishGameRequest()
        {
            GameId = game.GameId,
            WinningSequence = [1,2,25]
        };
        
        // Act
        var exception = await Assert.ThrowsAsync<ApiException>(async () =>
        {
            await client.FinishAsync(payload);
        });
        
        Assert.Equal(400, exception.StatusCode);
        Assert.Contains("All numbers in WinningSequence must be between 1 and 16", exception.Response);
    }
    
    [Fact(DisplayName = "Admin must finish the game with exactly 3 numbers")]
    public async Task GameFinish_AdminMustFinishGameBy3Numbers()
    {
        // Arrange
        var client = new Client(TestHttpAdmin);
        var game = TestObjects.GetGame();
        
        await PgCtxSetup.DbContextInstance.Games.AddAsync(game);
        await PgCtxSetup.DbContextInstance.SaveChangesAsync();
        
        var payload = new FinishGameRequest()
        {
            GameId = game.GameId,
            WinningSequence = [1,2,3,4]
        };
        
        // Act
        var exception = await Assert.ThrowsAsync<ApiException>(async () =>
        {
            await client.FinishAsync(payload);
        });
        
        
        Assert.Equal(400, exception.StatusCode);
        Assert.Contains("must contain exactly 3 unique numbers", exception.Response);
    }

    [Fact(DisplayName = "A regular user cannot finish a game")]
    public async Task GameFinish_UserCannotFinishGame()
    {
        // Arrange
        var client = new Client(TestHttpClient);
        var game = TestObjects.GetGame();
        
        await PgCtxSetup.DbContextInstance.Games.AddAsync(game);
        await PgCtxSetup.DbContextInstance.SaveChangesAsync();
        
        var payload = new FinishGameRequest()
        {
            GameId = game.GameId,
            WinningSequence = [1,2,3]
        };
        
        // Act
        var exception = await Assert.ThrowsAsync<ApiException>(async () =>
        {
            await client.FinishAsync(payload);
        });
        
        // Assert
        Assert.Equal(403, exception.StatusCode);
    }
    
    [Fact(DisplayName = "Only Logged in users can see game details")]
    public async Task GameDetails_OnlyLoggedInUsersAllowed()
    {
        // Arrange
        var client = new Client(TestHttpClient);
        var game = TestObjects.GetGame();
        
        await PgCtxSetup.DbContextInstance.Games.AddAsync(game);
        await PgCtxSetup.DbContextInstance.SaveChangesAsync();

        // Act
        var response = await client.Game2Async(game.GameId);
        
        // Assert
        Assert.Equal(200, response.StatusCode);
    }
    
    [Fact(DisplayName = "Non Logged in users cannot see the game details")]
    public async Task GameDetails_NonLoggedInUsersCannotSeeGameDetails()
    {
        // Arrange
        var client = new Client(CreateClient()); // User without jwt
        
        // Act
        var exception = await Assert.ThrowsAsync<ApiException>(async () =>
        {
            await client.GameAsync();
        });
        
        // Assert
        Assert.Equal(401, exception.StatusCode);
    }
    
    [Fact(DisplayName = "Admin can see all the games metadata")]
    public async Task GameDetails_AdminAllowedToSeeAllGamesMetaData()
    {
        // Arrange
        var client = new Client(TestHttpAdmin);
        var game = TestObjects.GetGame();
        
        await PgCtxSetup.DbContextInstance.Games.AddAsync(game);
        await PgCtxSetup.DbContextInstance.SaveChangesAsync();

        // Act
        var response = await client.LwAllAsync(game.Year);
        
        // Assert
        Assert.Equal(200, response.StatusCode);
    }
    
    
    [Fact(DisplayName = "Players cant see all the games metadata")]
    public async Task GameDetails_PlayersCantSeeMetaData()
    {
        // Arrange
        var client = new Client(TestHttpClient);
        var game = TestObjects.GetGame();
        
        await PgCtxSetup.DbContextInstance.Games.AddAsync(game);
        await PgCtxSetup.DbContextInstance.SaveChangesAsync();

        // Act
        var exception = await Assert.ThrowsAsync<ApiException>(async () =>
        {
            await client.LwAllAsync(game.Year);
        });
        
        // Assert
        Assert.Equal(403, exception.StatusCode);
    }
    
    [Fact(DisplayName = "Admin can see full game's metadata")]
    public async Task GameDetails_AdminAllowedToSeeMetaData()
    {
        // Arrange
        var client = new Client(TestHttpAdmin);
        var game = TestObjects.GetGame();
        
        await PgCtxSetup.DbContextInstance.Games.AddAsync(game);
        await PgCtxSetup.DbContextInstance.SaveChangesAsync();

        // Act
        var response = await client.LwAsync(game.GameId);
        
        // Assert
        Assert.Equal(200, response.StatusCode);
    }
    
    [Fact(DisplayName = "Player cant see full game's metadata")]
    public async Task GameDetails_PlayerNotAllowedToSeeMetaData()
    {
        // Arrange
        var client = new Client(TestHttpClient);
        var game = TestObjects.GetGame();
        
        await PgCtxSetup.DbContextInstance.Games.AddAsync(game);
        await PgCtxSetup.DbContextInstance.SaveChangesAsync();

        // Act
        var exception = await Assert.ThrowsAsync<ApiException>(async () =>
        {
            await client.LwAsync(game.GameId);
        });
        
        // Assert
        Assert.Equal(403, exception.StatusCode);
    }

    [Fact(DisplayName = "Admin can change game's offline properties")]
    public async Task OfflineProperty_AdminCanChange()
    {
        // Arrange
        var client = new Client(TestHttpAdmin);
        var game = TestObjects.GetGame();
        var payload = new UpdateOfflineProperties()
        {
            Players = 100,
            WinningBoards = 10,
            Boards = 400,
            WinningPlayers = 122,
            Income = 400,
            Payouts = 100
        };
        
        
        await PgCtxSetup.DbContextInstance.Games.AddAsync(game);
        await PgCtxSetup.DbContextInstance.SaveChangesAsync();

        // Act
        var response = await client.UpdateOfflineAsync(game.GameId, payload);
        
        // Assert
        Assert.Equal(200, response.StatusCode);
    }
    
    [Fact(DisplayName = "Users cant change game's offline properties")]
    public async Task OfflineProperty_UsersCantChange()
    {
        // Arrange
        var client = new Client(TestHttpClient);
        var game = TestObjects.GetGame();
        var payload = new UpdateOfflineProperties()
        {
            Players = 100,
            WinningBoards = 10,
            Boards = 400,
            WinningPlayers = 122,
            Income = 400,
            Payouts = 100
        };
        
        
        await PgCtxSetup.DbContextInstance.Games.AddAsync(game);
        await PgCtxSetup.DbContextInstance.SaveChangesAsync();
        
        // Act
        var exception = await Assert.ThrowsAsync<ApiException>(async () =>
        {
            await client.UpdateOfflineAsync(game.GameId, payload);
        });
        
        // Assert
        Assert.Equal(403, exception.StatusCode);
    }
    
}