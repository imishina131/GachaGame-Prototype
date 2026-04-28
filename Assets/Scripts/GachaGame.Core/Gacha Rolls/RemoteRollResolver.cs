using MatrixUtils.Attributes;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.CloudScriptModels;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Handles requesting a remote roll from the azure function in playfab. Outputs the result via <see cref="OnRollComplete"/>
/// </summary>
public class RemoteRollHandler : MonoBehaviour, IRollHandler
{
    [SerializeField, RequiredField] CharacterDataSO CharacterData;
    public UnityEvent<CharacterData> OnRollComplete = new();
    /// <summary>
    /// The ID of the banner that will be rolled
    /// </summary>
    [field:SerializeField] public string BannerID { get; private set; } = "Main Banner";
    /// <inheritdoc/>
    public void Roll()
    {
        PlayFabCloudScriptAPI.ExecuteFunction(
            new()
            {
                Entity = new()
                {
                    Id = PlayFabSettings.staticPlayer.EntityId,
                    Type = PlayFabSettings.staticPlayer.EntityType
                },
                FunctionName = "BannerRollRequest",
                FunctionParameter = new BannerRollRequestData(BannerID),
                GeneratePlayStreamEvent = true
            }, 
            ResultCallback,
            ErrorCallback
        );
    }
    /// <inheritdoc/>
    public void UpdateBannerToRoll(string bannerID)
    {
        BannerID = bannerID;
    }
    static void ErrorCallback(PlayFabError obj)
    {
        Debug.LogError(obj.GenerateErrorReport());
    }
    void ResultCallback(ExecuteFunctionResult obj)
    {
        Debug.Log(obj.FunctionResult);
        RollData data = JsonConvert.DeserializeObject<RollData>(obj.FunctionResult.ToString());
        SerializableGuid characterID = SerializableGuid.FromHexString(data.Character.ToString("N").ToUpper());
        if (!CharacterData.Characters.TryGetValue(characterID, out CharacterData characterData)) return;
        OnRollComplete.Invoke(characterData);
    }
    
}
