using System;
using UnityEngine;

[Serializable]
public class CharacterRoll : IRollData
{
    [SerializeField] SerializableGuid m_id;
    [field:SerializeField] public uint Rarity { get; private set;}
}