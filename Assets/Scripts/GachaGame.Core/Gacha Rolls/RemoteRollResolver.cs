using System;
using MatrixUtils.Attributes;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.CloudScriptModels;
using UnityEngine;
using UnityEngine.Events;

public class RemoteRollHandler : MonoBehaviour, IRollHandler
{
    [SerializeField, RequiredField] CharacterDataSO CharacterData;
    public UnityEvent<CharacterData> OnRollComplete;
    public void DefaultRoll()
    {
        Roll("Main Banner");
    }
    public CharacterDataSO Roll(string bannerID)
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
                FunctionParameter = new BannerRollRequestData(bannerID),
                GeneratePlayStreamEvent = true
            }, 
            ResultCallback,
            ErrorCallback
        );
        return null;
    }
    static void ErrorCallback(PlayFabError obj)
    {
        Debug.LogError(obj.GenerateErrorReport());
    }
    void ResultCallback(ExecuteFunctionResult obj)
    {
        RollResultData data = JsonConvert.DeserializeObject<RollResultData>(obj.FunctionResult.ToString());
        Debug.Log($"Result ID: {data.CharacterID}");
        if (!CharacterData.Characters.TryGetValue(data.CharacterID, out CharacterData characterData)) return;
        Debug.Log($"Rolled: {characterData.Name}");
        OnRollComplete.Invoke(characterData);
    }
    
}
