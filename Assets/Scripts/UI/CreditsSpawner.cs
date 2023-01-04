using System.Collections;
using UnityEngine;
using TMPro;

public class CreditsSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject CreditMentionPrefab;

    [SerializeField]
    string[] Credits;

    int creditsCounter = 0;

    [SerializeField]
    float firstCreditTimeStep = 0.5f;

    [SerializeField]
    float SpawnRate = 2f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForMissionStart());
    }

    IEnumerator SpawnFirstCredit()
    {
        yield return new WaitForSeconds(firstCreditTimeStep);
        SpawnCredit();
        StartCoroutine(SpawnCredits());
    }

    IEnumerator WaitForMissionStart()
    {
        yield return new WaitForSeconds(0.1f);

        if (!TheGame.theGameInst.MissionCanBegin)
            StartCoroutine(WaitForMissionStart());
        else
            StartCoroutine(SpawnFirstCredit());
    }

    IEnumerator SpawnCredits()
    {
        yield return new WaitForSeconds(SpawnRate);

        creditsCounter++;

        if (creditsCounter < Credits.Length)
        {
            SpawnCredit();
            StartCoroutine(SpawnCredits());        }

        
    }

    private void SpawnCredit()
    {
        GameObject nextCredit = CreditMentionPrefab;
        TMP_Text creditMention = nextCredit.GetComponentInChildren<TMP_Text>();
        creditMention.text = Credits[creditsCounter];
        Instantiate(nextCredit, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        
    }
}
