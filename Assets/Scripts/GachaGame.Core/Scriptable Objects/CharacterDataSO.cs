using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Scriptable Objects/Character Data")]
public class CharacterDataSO : ScriptableObject
{
    [field: SerializeField] public SerializableGuid ID { get; private set; } = SerializableGuid.NewGuid();
    [field: SerializeField] public GameObject Prefab { get; private set; }
}
