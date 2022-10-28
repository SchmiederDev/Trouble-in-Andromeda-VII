using UnityEngine;

[CreateAssetMenu(fileName = "new Level", menuName = "ScriptableObjects/Level")]
public class Level : ScriptableObject
{
    public string LevelName;
    public string LevelObjective;

    public int XPThreshold;
    public int TimeThreshold;

    public bool MissionObjectiveTime;
    public bool hasGatherObjective;

    public string gatherType;

    public bool UnlockLevelAccomplishment;
    public int UnlockIndex;
    public string UnlockText;

    public LevelSpawnObject[] LevelCollectables;
    public LevelSpawnEnemy[] LevelEnemies;
}
