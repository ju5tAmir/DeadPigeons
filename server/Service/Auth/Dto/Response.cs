namespace Service.Auth.Dto;

public record RegisterResponse(string UserId);

public record LoginResponse(string Jwt);

public record AuthUserInfo(string UserId, string FirstName, string LastName, string Username, string Email, string PhoneNumber, string Role,  bool IsActive, bool IsAutoplay, DateTime RegisterationDate);
