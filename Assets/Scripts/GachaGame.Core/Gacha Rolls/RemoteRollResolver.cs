using System;
using MatrixUtils.Attributes;
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
        if(!Guid.TryParse(obj.FunctionResult.ToString().Trim('"'), out Guid result)) return;
        if (!CharacterData.Characters.TryGetValue(result, out CharacterData characterData)) return;
        OnRollComplete.Invoke(characterData);
    }
}
