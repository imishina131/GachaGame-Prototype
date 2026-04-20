using System;
using System.Collections.Generic;
using MatrixUtils.Attributes;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.CloudScriptModels;
using UnityEngine;
using UnityEngine.Events;

public class RemoteRollHandler : MonoBehaviour, IRollHandler
{
    [SerializeField, RequiredField] CharacterDataSO CharacterData;
    public UnityEvent<CharacterData> OnRollComplete = new();
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
        Debug.Log(obj.FunctionResult);
        RollData data = JsonConvert.DeserializeObject<RollData>(obj.FunctionResult.ToString());
        SerializableGuid characterID = SerializableGuid.FromHexString(data.Character.ToString("N").ToUpper());
        if (!CharacterData.Characters.TryGetValue(characterID, out CharacterData characterData)) return;
        OnRollComplete.Invoke(characterData);
    }
    
}
