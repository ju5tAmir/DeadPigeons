namespace Service;

public static class Role
{
    public const string Admin = "Admin";
    public const string Player = "Player";
    
    public static string AllRoles => $"{Admin}, {Player}";
}