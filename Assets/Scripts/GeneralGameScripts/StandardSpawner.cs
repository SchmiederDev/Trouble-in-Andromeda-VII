using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform SpawnPoint;

    [SerializeField]
    private float spawnRange = 15f;


    [SerializeField]
    private List<GameObject> SpawnObjects;

    [SerializeField]
    private LevelSpawnObject[] LevelObjects;

    public void Init_SpawnObjects(LevelSpawnObject[] levelSpawnObjects)
    {
        LevelObjects = levelSpawnObjects;
        foreach (LevelSpawnObject spawnObject in LevelObjects)
        {
            GameObject NewSpawnObject = TheGame.theGameInst.Pickables.Find(SpawnElement => SpawnElement.name == spawnObject.LSO_Name);
            SpawnObjects.Add(NewSpawnObject);            
        }

        StartCoroutine(WaitForMissionStart());
    }

    private void Awake()
    {
        SpawnPoint = GetComponent<Transform>();
    }


    IEnumerator WaitForMissionStart()
    {
        yield return new WaitForSeconds(0.1f);
        if (!TheGame.theGameInst.MissionCanBegin)
        {
            
            StartCoroutine(WaitForMissionStart());
        }

        else
        {
            
            for (int i = 0; i < LevelObjects.Length; i++)
            {               
                StartCoroutine(SpawnObject(LevelObjects[i].LSO_SpawnRate, i));
            }
        }
    }

    IEnumerator SpawnObject(float spawnRate, int currentindex)
    {
        yield return new WaitForSeconds(spawnRate);
        
        if(SpawnObjects != null && SpawnObjects.Count > 0)
        {            
            GameObject NewObject = Instantiate(SpawnObjects[currentindex], new Vector3(Random.Range(-spawnRange, spawnRange), SpawnPoint.position.y, 0), Quaternion.identity);
        }

        StartCoroutine(SpawnObject(spawnRate, currentindex));
    }

    public void Clear_Lists()
    {        
        SpawnObjects.Clear();
    }
}
