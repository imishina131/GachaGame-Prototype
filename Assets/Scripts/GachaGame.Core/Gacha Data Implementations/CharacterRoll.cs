using System;
using UnityEngine;

[Serializable]
public class CharacterRoll : IRollData<float>
{
    [SerializeField] CharacterDataSO m_character;
    [field:SerializeField] public uint Rarity { get; private set;}
    public float Weight { get; private set;}
}