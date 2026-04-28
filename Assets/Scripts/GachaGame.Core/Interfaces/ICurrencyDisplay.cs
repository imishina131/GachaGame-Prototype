/// <summary>
/// Represents a system that can take in a <see cref="CurrencyInfoSO"/> and update data from playfab about the stored currency
/// </summary>
public interface ICurrencyDisplay
{
    /// <summary>
    /// Changes the <see cref="CurrencyInfoSO"/> displayed by this display
    /// </summary>
    /// <param name="currencyInfo"></param>
    void UpdateDisplayedCurrency(CurrencyInfoSO currencyInfo);
    /// <summary>
    /// Updates the data stored on the remote based on the current active currency in the display
    /// </summary>
    void UpdateActiveCurrency();
}
