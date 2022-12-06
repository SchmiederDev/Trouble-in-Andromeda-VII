using System.Collections;
using UnityEngine;

public class BattleTowerPhaserSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject EnemyWeapon;

    [SerializeField]
    private Transform PhaserSpawnPoint;

    [SerializeField]
    private float startingTime = 0.25f;

    [SerializeField]
    private float phaserSpawnRate = 0.5f;

    private void Start()
    {
        StartCoroutine(WaitStartingTime());
    }

    IEnumerator WaitStartingTime()
    {
        yield return new WaitForSeconds(startingTime);
        StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(phaserSpawnRate);
        SpawnPhaser();
        StartCoroutine(Fire());
    }

    private void SpawnPhaser()
    {
        GameObject Phaser = Instantiate(EnemyWeapon, PhaserSpawnPoint.position, Quaternion.identity, gameObject.transform);
        TheGame.theGameInst.audioManager.PlaySound("AlienPhaser_01");
    }
}
