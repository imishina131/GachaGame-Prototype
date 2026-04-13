using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BannerDataSO", menuName = "Scriptable Objects/Banner Data")]
public class BannerDataSO : ScriptableObject
{
    [field:SerializeField] public string BannerName { get; private set; }
    [field:SerializeField] public Sprite BannerIcon { get; private set; }
    [field:SerializeField] public Color BannerColor { get; private set; }
}
