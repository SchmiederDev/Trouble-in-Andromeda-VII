using UnityEngine;

[CreateAssetMenu(fileName = "new Enemy", menuName = "ScriptableObjects/Enemy")]
public class EnemyArchetype : ScriptableObject
{
    public string EnemyName;
    public int ImpactMagnitude = 5;
    public int StructuralIntegrity = 10;
    public int XP = 10;
}
