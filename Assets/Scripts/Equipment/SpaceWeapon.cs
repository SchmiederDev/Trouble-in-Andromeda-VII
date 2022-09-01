using UnityEngine;

[CreateAssetMenu(fileName = "new Space Weapon", menuName = "ScriptableObjects/Space Weapon")]
public class SpaceWeapon : ScriptableObject
{
    public string WeaponName;
    public int FirePower = 0;
    public int ShieldBreachCapacity = 0;
}
