using System;
using System.Collections.Generic;
using MatrixUtils.Attributes;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.CloudScriptModels;
using UnityEngine;
using UnityEngine.Events;
//using TMPro;

public class CurrencyHandler : MonoBehaviour
{
    //public TMP_Text coinsAmountText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetCurrency();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnLoginSuccess(LoginResult result)
    {
        GetCurrency();
    }

    public void GetCurrency()
    {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnError);
    }

    void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {
        int coins = result.VirtualCurrency["CN"];
        Debug.Log("Coins: " + coins);
        //coinsAmountText.text = coins.ToString();

    }

    public void AddCurrency()
    {
        var request = new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = "CN",
            Amount = 100
        };

        PlayFabClientAPI.AddUserVirtualCurrency(request, OnGiveVirtualCurrencySuccess, OnError);
    }

    void OnGiveVirtualCurrencySuccess(ModifyUserVirtualCurrencyResult result)
    {
        Debug.Log("Currency added");
        GetCurrency();
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Something went wrong: " + error.ErrorMessage);
    }
}
