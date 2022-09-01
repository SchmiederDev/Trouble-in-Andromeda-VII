using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform EnemySpawnPoint;

    [SerializeField]
    private float spawnRange = 15f;

    [SerializeField]
    private LevelSpawnEnemy[] EnemySpawnObjects;

    [SerializeField]
    private List<GameObject> Enemies;

    public float MinEnemyToEnemyDistance { get; } = 2f;

    private void Awake()
    {
        EnemySpawnPoint = GetComponent<Transform>();
    }

    public void Init_EnemiesToSpawn(LevelSpawnEnemy[] spawnEnemies)
    {
        EnemySpawnObjects = spawnEnemies;

        foreach(LevelSpawnEnemy spawnEnemy in EnemySpawnObjects)
        {
            GameObject NewEnemy = TheGame.theGameInst.Enemies.Find(EnemyElement => EnemyElement.name == spawnEnemy.LSO_Name);
            Enemies.Add(NewEnemy);
        }

        StartCoroutine(WaitForMissionStart());
    }
    

    public IEnumerator WaitForMissionStart()
    {
        yield return new WaitForSeconds(0.1f);
        if(!TheGame.theGameInst.MissionCanBegin)
        {
            StartCoroutine(WaitForMissionStart());
        }

        else
        {            
            for(int i = 0; i < EnemySpawnObjects.Length; i++)
            {
                StartCoroutine(SpawnEnemy(i, EnemySpawnObjects[i]));
            }
        }
    }

    public IEnumerator SpawnEnemy(int currentIndex, LevelSpawnEnemy currentEnemy)
    {
        yield return new WaitForSeconds(currentEnemy.LSO_SpawnRate);

        if(TheGame.theGameInst.CountEnemiesOfType(currentEnemy.LSO_Name) < currentEnemy.MaxNumberOnScene && currentEnemy != null)
        {
            if(currentEnemy.YRange == true)
            {
                Vector3 NewEnemyPos;

                do
                {
                    NewEnemyPos = new Vector3(Random.Range(-spawnRange, spawnRange), Random.Range(EnemySpawnPoint.position.y, EnemySpawnPoint.position.y - 12f), 0);
                } while (!TheGame.theGameInst.CheckIfEnemyYPosIsClear(NewEnemyPos));

                

                GameObject NewEnemy = Instantiate(Enemies[currentIndex], NewEnemyPos, EnemySpawnPoint.rotation);                
                TheGame.theGameInst.Add_Enemy(NewEnemy);
            }

            else
            {
                GameObject NewEnemy = Instantiate(Enemies[currentIndex], new Vector3(Random.Range(-spawnRange, spawnRange), EnemySpawnPoint.position.y, 0), EnemySpawnPoint.rotation);
                TheGame.theGameInst.Add_Enemy(NewEnemy);
            }           
            
        }

        StartCoroutine(SpawnEnemy(currentIndex, currentEnemy));
    }

    public void Clear_Lists()
    {
        Enemies.Clear();
    }
}
