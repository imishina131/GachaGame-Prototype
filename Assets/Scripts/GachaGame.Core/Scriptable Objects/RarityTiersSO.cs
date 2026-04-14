using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "Rarity Tiers", menuName = "Scriptable Objects/Rarity Tiers")]
public class RarityTiersSO : ScriptableObject
{
    [SerializeField] SerializableDictionary<SerializableGuid, RarityTier> m_rarities;
    public IReadOnlyDictionary<SerializableGuid, RarityTier> Rarities => m_rarities.Dictionary;
}
[Serializable]
public struct RarityTier
{
    [field:SerializeField] public Sprite Icon { get; private set; }
    [field:SerializeField] public string Name { get; private set; }
    [field:SerializeField] public Color Color { get; private set; }
}