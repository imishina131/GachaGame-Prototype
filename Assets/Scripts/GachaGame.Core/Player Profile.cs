using PlayFab.ClientModels;
public class PlayerProfile
{
    public string PlayFabId { get; set; }
    public string Username { get; set; }
    public string SessionTicket { get; set; }
    public EntityTokenResponse EntityToken { get; set; }

    public PlayerProfile(string playFabId, string username, string sessionTicket, EntityTokenResponse entityToken)
    {
        PlayFabId = playFabId;
        Username = username;
        SessionTicket = sessionTicket;
        EntityToken = entityToken;
    }
}
