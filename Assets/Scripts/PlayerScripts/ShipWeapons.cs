using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShipWeapons : MonoBehaviour
{
    [SerializeField]
    private Transform PhaserSpawnPoint;

    [SerializeField]
    private UnionFighter MyUnionFighter;

    [SerializeField]
    private GameObject ActiveWeapon;

    [SerializeField]
    private List<GameObject> SpaceWeapons;

    private bool ContralsEnabled = false;

    private bool IsFiring = false;

    private int spawnCounter = 0;

    [SerializeField]
    private int spawnRate = 15;

    public delegate void OnWeaponFired();
    public OnWeaponFired onWeaponFired;

    // Start is called before the first frame update
    void Start()
    {
        PhaserSpawnPoint = GetComponent<Transform>();
        MyUnionFighter = GetComponentInParent<UnionFighter>();
        Set_ActiveWeapon();
        MyUnionFighter.weaponChanged += Set_ActiveWeapon;
        TheGame.theGameInst.onMissionCanBegin += Enable_Controls;
    }

    private void FixedUpdate()
    {
        if(ContralsEnabled)
        {
            IsFiring = Input.GetButton("Fire1");

            if (IsFiring)
            {
                if (TheGame.theGameInst.PlayerUnionFighter.Energy > 0)
                {
                    if (spawnCounter == spawnRate)
                    {
                        SpawnPhaser();
                        onWeaponFired.Invoke();
                        spawnCounter = 0;
                    }

                    spawnCounter++;

                }
            }
        }
    }

    private void SpawnPhaser()
    {       
        GameObject Phaser = Instantiate(ActiveWeapon, PhaserSpawnPoint.position, Quaternion.identity);
        //TheGame.theGameInst.audioManager.PlaySound(ActiveWeapon.name);
        TheGame.theGameInst.audioManager.PlaySound("Phaser_Level_01");
    }

    private void Set_ActiveWeapon()
    {
        ActiveWeapon = SpaceWeapons.Find(WeaponElement => WeaponElement.name == TheGame.theGameInst.PlayerUnionFighter.ActiveWeapon.WeaponName);
    }

    private void Enable_Controls()
    {
        ContralsEnabled = TheGame.theGameInst.MissionCanBegin;
    }

}
