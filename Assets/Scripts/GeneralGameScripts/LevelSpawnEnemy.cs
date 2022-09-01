using UnityEngine;

[CreateAssetMenu(fileName = "new SpawnEnemy", menuName = "ScriptableObjects/LevelEnemy")]
public class LevelSpawnEnemy : LevelSpawnObject
{
    public int MaxNumberOnScene;
    public bool YRange;
}
