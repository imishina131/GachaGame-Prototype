using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Scriptable Objects/Character Data")]
public class CharacterDataSO : ScriptableObject
{
    [field: SerializeField] public SerializableDictionary<SerializableGuid, CharacterData> Characters { get; private set; } = new();
}
[Serializable]
public class CharacterData
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public GameObject Prefab { get; private set; }
}