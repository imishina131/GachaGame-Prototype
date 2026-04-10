using System;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "New Rarity Tier", menuName = "Scriptable Objects/Rarity Tier")]
public class RarityTier : ScriptableObject
{
    [SerializeField] SerializableGuid m_rarityID = SerializableGuid.NewGuid();
    public Guid RarityID => m_rarityID;
    [field:SerializeField] public Sprite Icon { get; private set; }
    [field:SerializeField] public string Name { get; private set; }
    [field:SerializeField] public Color Color { get; private set; }
}
