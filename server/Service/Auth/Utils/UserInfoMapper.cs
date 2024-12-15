using DataAccess.Entities;
using Service.Auth.Dto;

namespace Service.Auth.Utils;

public class UserInfoMapper
{
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