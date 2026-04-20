using MatrixUtils.Attributes;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class CurrencyHandler : MonoBehaviour
{
    [SerializeField, RequiredField] TMP_Text m_coinsAmountText;
    public void UpdateCurrency()
    {
        PlayFabClientAPI.GetUserInventory(new(), OnGetUserInventorySuccess, OnError);
    }
    void Start()
    {
        UpdateCurrency();
    }

    void OnGetUserInventorySuccess(GetUserInventoryResult result)
    {
        int coins = result.VirtualCurrency["CN"];
        m_coinsAmountText.text = coins.ToString();

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
        UpdateCurrency();
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Something went wrong: " + error.ErrorMessage);
    }
}
