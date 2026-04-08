using System;
using UnityEngine;

[Serializable, CreateAssetMenu(fileName = "New Rarity Tier", menuName = "Scriptable Objects/Rarity Tier")]
public class RarityTier : ScriptableObject, IComparable<RarityTier>
{
    [field:SerializeField] public uint Value { get; private set; }
    [field:SerializeField] public Sprite Icon { get; private set; }
    [field:SerializeField] public string Name { get; private set; }
    [field:SerializeField] public Color Color { get; private set; }
    public int CompareTo(RarityTier other)
    {
        if (ReferenceEquals(this, other)) return 0;
        return other is null ? 1 : Value.CompareTo(other.Value);
    }
}
