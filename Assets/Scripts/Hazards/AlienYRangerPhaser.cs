using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienYRangerPhaser : MonoBehaviour
{
    [SerializeField]
    private GameObject EnemyWeapon;

    [SerializeField]
    private Transform PhaserSpawnPoint;

    [SerializeField]
    private float phaserSpawnRate = 1f;

    private void Start()
    {
        StartCoroutine(Fire());
    }

    private void SpawnPhaser()
    {
        GameObject Phaser = Instantiate(EnemyWeapon, PhaserSpawnPoint.position, Quaternion.identity);
        TheGame.theGameInst.audioManager.PlaySound("AlienPhaser_01");
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(phaserSpawnRate);
        SpawnPhaser();
        StartCoroutine(Fire());
    }
}
