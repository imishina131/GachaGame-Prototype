using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Scriptable Objects/Character Data")]
public class CharacterDataSO : ScriptableObject
{
    [SerializeField] SerializableDictionary<SerializableGuid, CharacterData> m_characters = new();
    public IReadOnlyDictionary<SerializableGuid, CharacterData> Characters => m_characters.Dictionary;
    
}
[Serializable]
public class CharacterData
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public GameObject Prefab { get; private set; }
}