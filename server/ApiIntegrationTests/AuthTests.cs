using Generated;

namespace ApiIntegrationTests;

public class AuthTests : ApiTestBase
{
    [Fact(DisplayName = "Admin can register new users successfully")]
    public async Task Register_AdminCanRegisterNewUsers()
    {
        // Arrange
        var adminClient = new Client(TestHttpAdmin);
        var user = TestObjects.GetUser();

        var payload = new RegisterRequest
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
        
        // Act
        var response = await adminClient.RegisterAsync(payload);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(200, response.StatusCode); 
        Assert.NotNull(response.Result.UserId);
    }

    [Fact(DisplayName = "Regular users cannot register new users")]
    public async Task Register_UserCannotRegisterNewUsers()
    {
        // Arrange
        var userClient = new Client(TestHttpClient);
        var user = TestObjects.GetUser();

        var payload = new RegisterRequest
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApiException>(async () => 
        {
            await userClient.RegisterAsync(payload);
        });

        // Verify the exception details
        Assert.Equal(403, exception.StatusCode);
    }
    
    [Fact(DisplayName = "Users can log in with valid credentials")]
    public async Task Login_UsersCanLoginWithValidCredentials()
    {
        // Arrange
        var clientWithoutJwt = new Client(CreateClient());
        var payload = new LoginRequest()
        {
            Email = "player1@example.com",
            Password = "S3cret!"
        };

        // Act
        var response = await clientWithoutJwt.LoginAsync(payload);

        // Assert
        Assert.Equal(200, response.StatusCode);
        Assert.NotNull(response.Result.Jwt);
    }
    
    [Fact(DisplayName = "Login fails with invalid credentials")]
    public async Task Login_UsersLoginFailsWithInvalidCredentials()
    {
        // Arrange
        var clientWithoutJwt = new Client(CreateClient());
        var payload = new LoginRequest
        {
            Email = "player1@example.com",
            Password = "InvalidPassword!"
        };

        // Act
        var exception = await Assert.ThrowsAsync<ApiException>(async () => 
        {
            await clientWithoutJwt.LoginAsync(payload);
        });

        // Assert
        Assert.NotNull(exception);
        Assert.NotEqual(401, exception.StatusCode); 
        Assert.Contains("Unable to authenticate", exception.Response); 
    }

    [Fact(DisplayName = "UserInfo endpoint returns valid user details")]
    public async Task UserInfo_EndpointReturnsValidUserDetails()
    {
        // Arrange
        var client = new Client(TestHttpClient);

        // Act
        var response = await client.UserinfoAsync();

        // Assert
        Assert.Equal(200, response.StatusCode);
        Assert.NotNull(response); // Ensure the response is not null
        Assert.NotNull(response.Result); // Ensure the result object is not null

        Assert.False(string.IsNullOrWhiteSpace(response.Result.UserId), "UserId should not be empty or whitespace.");
        Assert.False(string.IsNullOrWhiteSpace(response.Result.Email), "Email should not be empty or whitespace.");
        Assert.False(string.IsNullOrWhiteSpace(response.Result.Username), "Username should not be empty or whitespace.");
    }
    
    [Fact(DisplayName = "Logout endpoint successfully logs the user out")]
    public async Task Logout_EndpointSuccessfullyLogsUserOut()
    {
        // Arrange
        var client = new Client(TestHttpClient);

        // Act
        var response = await client.LogoutAsync();

        // Assert
        Assert.Equal(200, response.StatusCode); // Ensure the HTTP status code is 200 (OK)
    }
}
