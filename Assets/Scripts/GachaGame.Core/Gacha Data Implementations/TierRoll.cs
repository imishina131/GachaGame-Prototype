using System;
using System.Collections.Generic;
using MatrixUtils.Attributes;
using UnityEngine;

[Serializable]
public class TierRoll : IRollData<float>
{
    [SerializeField] List<CharacterRoll> m_characters;
    [SerializeReference, ClassSelector] IRollResolver<CharacterRoll, float> m_characterResolver;
    [SerializeField] RarityTier m_tier;
    [field:SerializeField] public uint Rarity { get; private set;}
    public float Weight { get; private set; }
}