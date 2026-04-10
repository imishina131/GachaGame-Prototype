using System;
using UnityEngine;

[Serializable]
public class CharacterRoll : IRollData
{
    [SerializeField] CharacterDataSO m_character;
    [field:SerializeField] public uint Rarity { get; private set;}
}