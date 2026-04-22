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
    
    public void UpdateDisplayedCurrency(CurrencyInfoSO currencyInfo)
    {
        m_activeCurrencyInfo = currencyInfo;
        CurrencyIcon.texture = currencyInfo.CurrencyIcon;
        UpdateActiveCurrency();
    }
    
    public void UpdateActiveCurrency()
    {
        PlayFabClientAPI.GetUserInventory(new(), OnGetUserInventorySuccess, OnError);
    }
    
    void Start()
    {
        UpdateActiveCurrency();
    }

    void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {
        int coins = result.VirtualCurrency[m_activeCurrencyInfo.CurrencyCode];
        CurrencyText.text = coins.ToString();

    }

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
