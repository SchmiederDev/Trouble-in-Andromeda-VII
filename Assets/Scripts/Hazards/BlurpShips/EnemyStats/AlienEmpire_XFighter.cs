using System;
using UnityEngine;

public class AlienEmpire_XFighter : Enemy
{
    [SerializeField]
    private float AttackRange = 1.5f;

    float PlayerX;

    float XDistance;

    bool IsWithinRange = false;

    [SerializeField]
    private GameObject EnemyWeapon;

    [SerializeField]
    private Transform PhaserSpawnPoint;

    private int phaserSpawnCounter = 0;

    [SerializeField]
    private float phaserSpawnRate = 10f;

    // Update is called once per frame
    void Update()
    {
        Check_EnemyPosition();        
    }

    private void FixedUpdate()
    {
        if (IsWithinRange)
            Fire();
    }

    private void Check_EnemyPosition()
    {
        PlayerX = TheGame.theGameInst.PlayerUnionFighter.FighterPosition.x;

        XDistance = Math.Abs(PlayerX - gameObject.transform.position.x);

        if (XDistance <= AttackRange)
        {
            IsWithinRange = true;
        }
            

        else IsWithinRange = false;
    }

    private void Fire()
    {
        if (phaserSpawnCounter == phaserSpawnRate)
        {
            SpawnPhaser();
            phaserSpawnCounter = 0;
        }

        phaserSpawnCounter++;
    }

    private void SpawnPhaser()    
    {         
        GameObject Phaser = Instantiate(EnemyWeapon, PhaserSpawnPoint.position, Quaternion.identity);
        TheGame.theGameInst.audioManager.PlaySound("AlienPhaser_01");        
    }
}
