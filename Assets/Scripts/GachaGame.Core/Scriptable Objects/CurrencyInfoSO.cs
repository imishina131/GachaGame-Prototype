using UnityEngine;
using UnityEngine.Serialization;
/// <summary>
/// Represents data for a currency stored on the remote playfab server
/// </summary>
[CreateAssetMenu(fileName = "Currency Info", menuName = "Scriptable Objects/Currency Info")]
public class CurrencyInfoSO : ScriptableObject
{
    /// <summary>
    /// The name of the currency
    /// </summary>
    [field:SerializeField] public string CurrencyName { get; private set; }
    /// <summary>
    /// The 2 character code used to look up the currency on playfab
    /// </summary>
    [field:SerializeField] public string CurrencyCode { get; private set; }
    /// <summary>
    /// The icon associated with this currency
    /// </summary>
    [field:SerializeField] public Texture2D CurrencyIcon { get; private set; }
}
