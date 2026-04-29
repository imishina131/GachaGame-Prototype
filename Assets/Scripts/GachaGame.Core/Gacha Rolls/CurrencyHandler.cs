//Irina Mishina
//Gacha Game Prototype
//2026-04-28
using MatrixUtils.Attributes;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyHandler : MonoBehaviour, ICurrencyDisplay
{
    [SerializeField] CurrencyInfoSO m_activeCurrencyInfo;
    [field: SerializeField, RequiredField] public TMP_Text CurrencyText { get; private set;}
    [field: SerializeField, RequiredField] public RawImage CurrencyIcon { get; private set;}
    
    public void UpdateDisplayedCurrency(CurrencyInfoSO currencyInfo)//updates currency displayed for the player
    {
        m_activeCurrencyInfo = currencyInfo;
        CurrencyIcon.texture = currencyInfo.CurrencyIcon;
        UpdateActiveCurrency();
    }

    /// <summary>
    /// updates the currency stored in playfab for the current player
    /// </summary>
    public void UpdateActiveCurrency()
    {
        PlayFabClientAPI.GetUserInventory(new(), OnGetUserInventorySuccess, OnError);
    }
    
    void Start()
    {
        UpdateActiveCurrency();
    }

    /// <summary>
    /// gets currency from playfab and displays it to the player
    /// </summary>
    void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {
        int coins = result.VirtualCurrency[m_activeCurrencyInfo.CurrencyCode];
        CurrencyText.text = coins.ToString();

    }

    /// <summary>
    /// when button pressed, adds 100 currency and updates the virtual part on playfab
    /// </summary>
    public void AddCurrency()
    {
        AddUserVirtualCurrencyRequest request = new()
        {
            VirtualCurrency = m_activeCurrencyInfo.CurrencyCode,
            Amount = 100
        };

        PlayFabClientAPI.AddUserVirtualCurrency(request, OnGiveVirtualCurrencySuccess, OnError);
    }

    void OnGiveVirtualCurrencySuccess(ModifyUserVirtualCurrencyResult result)
    {
        UpdateActiveCurrency();
    }

    void OnError(PlayFabError error)
    {
        Debug.LogError("Something went wrong: " + error.GenerateErrorReport());
    }
}
