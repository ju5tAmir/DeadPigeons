namespace Service.Game;

public class GameProfits
{
    public static readonly decimal OrganizerProfit;
    public static readonly decimal UserProfit;

    static GameProfits()
    {
        // Set dynamic values at runtime
        OrganizerProfit = 30.00m;
        UserProfit = 70.00m;
    }
}