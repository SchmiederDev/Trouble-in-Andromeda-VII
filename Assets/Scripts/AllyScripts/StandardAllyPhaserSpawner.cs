using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardAllyPhaserSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject AllyWeapon;

    [SerializeField]
    private Transform PhaserSpawnPoint;

    [SerializeField]
    private float missionStartTimeRate = 0.01f;

    [SerializeField]
    private float startingTime = 3f;

    [SerializeField]
    private float phaserSpawnRate = 0.5f;

    private bool IsWithinRange = false;

    [SerializeField]
    private float attackRange = 1.5f;

    [SerializeField]
    private List<GameObject> Targets;

    private GameObject ActiveTarget;

    [SerializeField]
    private List<string> TargetPriorities;

    private bool IsFirstTarget = true;
    private int targetIndex = 0;
    private int lastTargetCount = 0;
    private int newTargetCount = 0;

    UnionTankInitializer UT_Initializer;

    private void Start()
    {
        UT_Initializer = GetComponentInParent<UnionTankInitializer>();
        attackRange = UT_Initializer.attackRange;
        TargetPriorities = UT_Initializer.targetPriorities;

        TheGame.theGameInst.onActiveEnemiesChanged += Set_Targets;
        StartCoroutine(WaitForMissionStart());
    }

    private void Update()
    {
        if(ActiveTarget != null)
        {   
            float distance = ActiveTarget.transform.position.x - gameObject.transform.position.x;

            if (distance <= attackRange)
                IsWithinRange = true;
            else
                IsWithinRange = false;
        }
    }

    IEnumerator WaitForMissionStart()
    {
        yield return new WaitForSeconds(missionStartTimeRate);
        
        if (TheGame.theGameInst.MissionCanBegin)
            StartCoroutine(WaitStartingTime());
        else
            StartCoroutine(WaitForMissionStart());
    }

    IEnumerator WaitStartingTime()
    {
        yield return new WaitForSeconds(startingTime);
        StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(phaserSpawnRate);
        
        if(IsWithinRange && Targets.Count > 0)
            SpawnPhaser();

        StartCoroutine(Fire());
    }

    private void SpawnPhaser()
    {
        GameObject Phaser = Instantiate(AllyWeapon, PhaserSpawnPoint.position, Quaternion.identity, gameObject.transform);
        TheGame.theGameInst.audioManager.PlaySound("Phaser_Level_01");
    }

    private void Set_Targets()
    {
        Targets = TheGame.theGameInst.Get_ActiveEnemiesGameObjects();
        newTargetCount = Targets.Count;

        if(Targets != null && Targets.Count > 0)
            FindActiveEnemy();
    }

    private void FindActiveEnemy()
    {
        if(IsFirstTarget)
        {
            ActiveTarget = Targets.Find(targetElement => targetElement.name == TargetPriorities[targetIndex]);
            
            if(ActiveTarget != null)
            {                
                IsFirstTarget = false;
                lastTargetCount = Targets.Count;
            }
        }

        else
        {
            if(newTargetCount < lastTargetCount)
            {
                targetIndex++;

                if (targetIndex < TargetPriorities.Count)
                    FindNextTarget();

                else
                {                    
                    targetIndex = TargetPriorities.Count - 1;
                    FindNextTarget();
                }

                lastTargetCount = newTargetCount;
            }
        }
    }

    private void FindNextTarget()
    {
        string nextTarget = TargetPriorities[targetIndex];
        ActiveTarget = Targets.Find(targetElement => targetElement.name == nextTarget);

        if (ActiveTarget != null)
            Debug.Log("Next Target, Sender: " + gameObject.name + " Active Target: " + ActiveTarget.name);
        else
        {
            Debug.Log("No target found.");
            if (Targets.Count == 0)
                Debug.Log("No Enemies on Scene.");
        }
    }
}
