using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlurpXRangerAllyDestroyer : Enemy
{
    [SerializeField]
    private float AttackRange = 1.5f;

    float PlayerX;
    float PlayerEnemyDistance;

    private List<Transform> PlayerAllies;

    bool IsWithinRange = false;
    bool[] IsWithinAllyRange;

    [SerializeField]
    private GameObject EnemyWeapon;

    [SerializeField]
    private Transform PhaserSpawnPoint;

    private int phaserSpawnCounter = 0;

    [SerializeField]
    private float phaserSpawnRate = 10f;

    List<GameObject> AlliedShips;

    private void Start()
    {
        AlliedShips = new List<GameObject>();
        FindAlliedShips();
        InitRangeBools();
    }

    // Update is called once per frame
    void Update()
    {
        Check_EnemyPosition();
    }

    private void FixedUpdate()
    {
        if (IsWithinRange)
            Fire();

        for(int i = 0; i < IsWithinAllyRange.Length; i++)
        {
            if (IsWithinAllyRange[i])
                Fire();
        }
    }   

    private void Check_EnemyPosition()
    {
        CheckPlayerEnemyDistance();
        CheckAlliesEnemyDistance();        
    }

    private void CheckPlayerEnemyDistance()
    {
        PlayerX = TheGame.theGameInst.PlayerUnionFighter.FighterPosition.x;
        PlayerEnemyDistance = Math.Abs(PlayerX - gameObject.transform.position.x);


        if (PlayerEnemyDistance <= AttackRange) 
            IsWithinRange = true;
        else 
            IsWithinRange = false;
    }

    private void CheckAlliesEnemyDistance()
    {
        foreach (GameObject AllyShip in AlliedShips)
        {
            float enemyAllyDistance = Math.Abs(AllyShip.transform.position.x - gameObject.transform.position.x);

            int allyIndex = AlliedShips.IndexOf(AllyShip);

            if (enemyAllyDistance <= AttackRange)
                IsWithinAllyRange[allyIndex] = true;

            else
                IsWithinAllyRange[allyIndex] = false;
        }
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
    private void FindAlliedShips()
    {
        AlliedShips.Clear();
        Scene scene = SceneManager.GetActiveScene();
        GameObject[] GameObjectsOnScene = new GameObject[scene.GetRootGameObjects().Length];
        GameObjectsOnScene = scene.GetRootGameObjects();

        foreach (GameObject gameObject in GameObjectsOnScene)
        {
            if (gameObject.tag == "AllyShip")
            {
                AlliedShips.Add(gameObject);
            }
        }
    }

    private void InitRangeBools()
    {
        IsWithinAllyRange = new bool[AlliedShips.Count];
    }
}
