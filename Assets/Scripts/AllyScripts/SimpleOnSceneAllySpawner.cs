using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleOnSceneAllySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject AllyShip;

    [SerializeField]
    private Transform AllySpawnPoint;

    [SerializeField]
    private float waitingTimeRate = 0.1f;

    [SerializeField] 
    private float updateTimeRate = 1f;

    [SerializeField]
    private float respawnTime = 0.5f;   
    

    private bool shouldSpawn = true;
    public bool shouldRespawn { set; get; } = false;

    [SerializeField]
    private float maxNumberOfAlliesOnScene = 2;

    [SerializeField]
    private float maxNumberOfShips = 3f;

    private float spawnedShips = 0;
    private float spawnedShipsThreshold;

    // Start is called before the first frame update
    void Awake()
    {
        AllySpawnPoint = GetComponent<Transform>();

        spawnedShipsThreshold = maxNumberOfShips + 1;

        TheGame.theGameInst.onActiveAlliesChanged += UpdateAlliesOnScene;
    }

    private void Start()
    {        
        StartCoroutine(WaitForMissionStart());
    }

    IEnumerator WaitForMissionStart()
    {
        yield return new WaitForSeconds(waitingTimeRate);

        if(!TheGame.theGameInst.MissionCanBegin)
            StartCoroutine(WaitForMissionStart());
        else
            StartCoroutine(SpawnShip());
    }

    private void UpdateAlliesOnScene()
    {
        int currentNumberOfAlliesOnScene = TheGame.theGameInst.ActiveAllies.Count;

        if (currentNumberOfAlliesOnScene < maxNumberOfAlliesOnScene)
            shouldSpawn = true;
        else
            shouldSpawn = false;
    }

    private IEnumerator SpawnShip()
    {
        yield return new WaitForSeconds(respawnTime);
        GameObject NewAlly = Instantiate(AllyShip, AllySpawnPoint.position, AllySpawnPoint.rotation, AllySpawnPoint);
        TheGame.theGameInst.ActiveAllies.Add(NewAlly);

        if (spawnedShips < spawnedShipsThreshold)
            spawnedShips++;

        StartCoroutine(SpawnAllies());
    }

    private void SpawnAlly()
    { 
        if (spawnedShips < maxNumberOfShips && shouldRespawn)
        {
            StartCoroutine(SpawnShip());
            shouldRespawn = false;
        }

        shouldSpawn = false;

        StartCoroutine(SpawnAllies());
    }

    private IEnumerator SpawnAllies()
    {
        yield return new WaitForSeconds(updateTimeRate);

        if (shouldSpawn)
            SpawnAlly();
        else
            StartCoroutine(SpawnAllies());
    }

}
