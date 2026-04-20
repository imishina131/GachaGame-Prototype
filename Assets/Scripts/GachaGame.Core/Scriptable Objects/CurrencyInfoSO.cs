using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Currency Info", menuName = "Scriptable Objects/Currency Info")]
public class CurrencyInfoSO : ScriptableObject
{
    [field:SerializeField] public string CurrencyName { get; private set; }
    [field:SerializeField] public string CurrencyCode { get; private set; }
    [field:SerializeField] public Texture2D CurrencyIcon { get; private set; }
}
