using UnityEngine;

[CreateAssetMenu(fileName = "new SpawnObject", menuName = "ScriptableObjects/LevelObject")]
public class LevelSpawnObject : ScriptableObject
{
    public string LSO_Name;
    public float LSO_SpawnRate;
}
