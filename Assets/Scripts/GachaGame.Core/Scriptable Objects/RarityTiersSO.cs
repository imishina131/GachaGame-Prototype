using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Represents a set of <see cref="RarityTier"/> to be looked up by their assigned <see cref="Guid"/>
/// </summary>
[Serializable, CreateAssetMenu(fileName = "Rarity Tiers", menuName = "Scriptable Objects/Rarity Tiers")]
public class RarityTiersSO : ScriptableObject
{
    [SerializeField] SerializableDictionary<SerializableGuid, RarityTier> m_rarities;
    /// <summary>
    /// The <see cref="IReadOnlyDictionary{TKey,TValue}"/> used to look up a <see cref="RarityTier"/> from its assigned <see cref="Guid"/>
    /// </summary>
    public IReadOnlyDictionary<SerializableGuid, RarityTier> Rarities => m_rarities.Dictionary;
}
/// <summary>
/// Represents a rarity tier in the gacha system
/// </summary>
[Serializable]
public struct RarityTier
{
    /// <summary>
    /// The <see cref="Sprite"/> associated with this rarity tier
    /// </summary>
    [field:SerializeField] public Sprite Icon { get; private set; }
    /// <summary>
    /// The name of this rarity tier
    /// </summary>
    [field:SerializeField] public string Name { get; private set; }
    /// <summary>
    /// The <see cref="Color"/> associated with this rarity tier
    /// </summary>
    [field:SerializeField] public Color RarityColor { get; private set; }
}