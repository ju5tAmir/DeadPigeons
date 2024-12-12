using DataAccess.Entities;
using Service.Auth.Dto;
using Service.Preference.Utils;

namespace Service.Auth.Utils;

public class UserInfoMapper
{
    public static UserInfoResponse ToResponse(User user, string role, DataAccess.Entities.Preference preference)
    {
        var userInfo = new UserInfo(
            UserId: user.Id,
            FirstName: user.FirstName,
            LastName: user.LastName,
            Username: user.UserName,
            Email: user.Email,
            PhoneNumber: user.PhoneNumber,
            Balance: user.Balance,
            Role: role,
            IsActive: user.IsActive,
            IsAutoplay: user.IsAutoPlay,
            RegisterationDate: user.RegistrationDate
        );

        var preferenceResponse = PreferenceMapper.ToResponse(preference);

        return new UserInfoResponse(userInfo, preferenceResponse);
    }
    
    public static UserInfo ToResponse(User user, string role)
    {
        var userInfo = new UserInfo(
            UserId: user.Id,
            FirstName: user.FirstName,
            LastName: user.LastName,
            Username: user.UserName,
            Email: user.Email,
            PhoneNumber: user.PhoneNumber,
            Balance: user.Balance,
            Role: role,
            IsActive: user.IsActive,
            IsAutoplay: user.IsAutoPlay,
            RegisterationDate: user.RegistrationDate
        );
        

        return userInfo;
    }

    public static UserPiiResponse ToPiiResponse(User user)
    {
        var userInfo = new UserPiiResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.UserName
        );

        return userInfo;
    }
}