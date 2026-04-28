
using UnityEngine;
/// <summary>
/// Represents a Banner that can be selected in the gacha UI
/// </summary>
[CreateAssetMenu(fileName = "BannerDataSO", menuName = "Scriptable Objects/Banner Data")]
public class BannerDataSO : ScriptableObject
{
    /// <summary>
    /// The name of the banner (Used to loom up the correct banner on the remote)
    /// </summary>
    [field:SerializeField] public string BannerName { get; private set; }
    /// <summary>
    /// The <see cref="CurrencyInfoSO"/> of the currency used to roll this banner
    /// </summary>
    [field:SerializeField] public CurrencyInfoSO BannerCurrency { get; private set; }
    /// <summary>
    /// The <see cref="Color"/> that the background will change to when this banner is selected
    /// </summary>
    [field:SerializeField] public Color BannerBackgroundColor { get; private set; }
    /// <summary>
    /// The <see cref="Color"/> that the banner roll area will change to when this banner is selected
    /// </summary>
    [field:SerializeField] public Color BannerRollAreaColor { get; private set; }
    /// <summary>
    /// The <see cref="Color"/> that the banner roll button will change to when this banner is selected
    /// </summary>
    [field:SerializeField] public Color BannerRollButtonColor { get; private set; }
    /// <summary>
    /// The <see cref="Color"/> that the banner details button will change to when this banner is selected
    /// </summary>
    [field:SerializeField] public Color BannerDetailsButtonColor { get; private set; }
}
