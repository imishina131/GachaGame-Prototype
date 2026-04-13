using System;
using PlayFab;
using PlayFab.CloudScriptModels;
using UnityEngine;

public class RemoteRollHandler : MonoBehaviour, IRollHandler
{
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
                FunctionParameter = new BannerRollRequestData(bannerID)
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
    }
}
