using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface ICurrencyDisplay
{
    void UpdateDisplayedCurrency(CurrencyInfoSO currencyInfo);
    void UpdateActiveCurrency();
}
