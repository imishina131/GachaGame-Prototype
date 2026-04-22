using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BannerDataSO", menuName = "Scriptable Objects/Banner Data")]
public class BannerDataSO : ScriptableObject
{
    [field:SerializeField] public string BannerName { get; private set; }
    [field:SerializeField] public CurrencyInfoSO BannerCurrency { get; private set; }
    [field:SerializeField] public Color BannerBackgroundColor { get; private set; }
    [field:SerializeField] public Color BannerRollAreaColor { get; private set; }
    [field:SerializeField] public Color BannerRollButtonColor { get; private set; }
    [field:SerializeField] public Color BannerDetailsButtonColor { get; private set; }
}
