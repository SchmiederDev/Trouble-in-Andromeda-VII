using System.Collections;
using UnityEngine;

public class EndbossSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform EndbossSpawnPoint;

    [SerializeField]
    private GameObject EndbossObject;

    private float waitingTimeRate = 0.1f;

    [SerializeField]
    private float spawnTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForMissionStart());
    }

    IEnumerator WaitForMissionStart()
    {
        yield return new WaitForSeconds(waitingTimeRate);

        if(!TheGame.theGameInst.MissionCanBegin)
            StartCoroutine(WaitForMissionStart());
        else
            StartCoroutine(SpawnBoss());
    }

    IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(spawnTime);
        GameObject Endboss = Instantiate(EndbossObject, EndbossSpawnPoint.position, EndbossSpawnPoint.rotation);
    }
}
