using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a set of <see cref="CharacterData"/> to be looked up by their assigned <see cref="Guid"/>
/// </summary>
[CreateAssetMenu(fileName = "New Character Data", menuName = "Scriptable Objects/Character Data")]
public class CharacterDataSO : ScriptableObject
{
    [SerializeField] SerializableDictionary<SerializableGuid, CharacterData> m_characters = new();
    /// <summary>
    /// The <see cref="IReadOnlyDictionary{TKey,TValue}"/> used to look up <see cref="CharacterData"/> from its assigned <see cref="Guid"/>
    /// </summary>
    public IReadOnlyDictionary<SerializableGuid, CharacterData> Characters => m_characters.Dictionary;
    
}
/// <summary>
/// Represents a character that can be rolled in the gacha system
/// </summary>
[Serializable]
public class CharacterData
{
    /// <summary>
    /// The name of the character
    /// </summary>
    [field: SerializeField] public string Name { get; private set; }
    /// <summary>
    /// The prefab for the character's visual in the roll screen
    /// </summary>
    [field: SerializeField] public GameObject Prefab { get; private set; }
}