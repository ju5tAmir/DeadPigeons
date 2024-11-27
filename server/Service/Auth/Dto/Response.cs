using Service.Preference.Dto;

namespace Service.Auth.Dto;

public record RegisterResponse(string UserId);

public record LoginResponse(string Jwt);

public record UserInfo(string UserId, string FirstName, string LastName, string Username, string Email, string PhoneNumber, string Role,  bool IsActive, bool IsAutoplay, DateTime RegisterationDate);
public record UserInfoResponse(
    UserInfo UserInfo,
    PreferenceResponse UserPreference
    );

public record UserPiiResponse( // Personal Identifiable Information
    string UserId, string FirstName, string LastName, string Username
);

public record ConfirmResponse(string PasswordToken);