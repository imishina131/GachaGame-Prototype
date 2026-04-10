using System;
using System.Collections.Generic;
using MatrixUtils.Attributes;
using UnityEngine;

[Serializable]
public class TierRoll : IRollData
{
    [SerializeField] List<CharacterRoll> m_characters;
    [SerializeReference, ClassSelector] IRollResolver<CharacterRoll> m_characterResolver;
    [SerializeField] RarityTier m_tier;
    [field:SerializeField] public uint Rarity { get; private set;}
}