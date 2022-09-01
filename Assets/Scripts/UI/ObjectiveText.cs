using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text ObjectiveTxt;

    string missionObjective;

    char[] missionObjChars;

    int charCounter = 0;

    float ObjectiveTextAlpha = 0;

    [SerializeField]
    float fadeInRate = 0.025f;
    [SerializeField]
    float fadeOutRate = 0.01f;

    [SerializeField]
    float fadeInTimeSpan = 0.1f;
    [SerializeField]
    float fadeOutTimeSpan = 0.2f;

    [SerializeField]
    float textSpeed = 0.035f;

    [SerializeField]
    float blipSoundSpeed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        ObjectiveTxt = GetComponent<TMP_Text>();

        //Init_MissionObjective();
        //Play_MissionObjective();        
    }

    public void Init_MissionObjective()
    {
        ObjectiveTxt.text = null;
        charCounter = 0;
        missionObjective = TheGame.theGameInst.Get_CurrentMissionObjective();

        missionObjChars = new char[missionObjective.Length];
        missionObjChars = missionObjective.ToCharArray();        
    }

    public void Play_MissionObjective()
    {        
        StartCoroutine(Fade());
        StartCoroutine(AddLettersToObjective());
        StartCoroutine(PlayBlipSound());
    }

    IEnumerator Fade()
    {        
        yield return new WaitForSeconds(fadeInTimeSpan);

        if (ObjectiveTextAlpha <= 1f)
        {
            FadeIn();
            StartCoroutine(Fade());
        }

        else
        {
            StartCoroutine(FadeOut());
        }

    }

    private void FadeIn()
    {
        ObjectiveTextAlpha += fadeInRate;
        ObjectiveTxt.color = new Color(ObjectiveTxt.color.r, ObjectiveTxt.color.g, ObjectiveTxt.color.b, ObjectiveTextAlpha);
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(fadeOutTimeSpan);

        if (ObjectiveTextAlpha > 0)
        {
            ObjectiveTextAlpha -= fadeOutRate;
            ObjectiveTxt.color = new Color(ObjectiveTxt.color.r, ObjectiveTxt.color.g, ObjectiveTxt.color.b, ObjectiveTextAlpha);            

            StartCoroutine(FadeOut());
        }
        else
        {
            TheGame.theGameInst.MissionCanBegin = true;
        }
    }

    IEnumerator AddLettersToObjective()
    {
        yield return new WaitForSeconds(textSpeed);
        
        if (charCounter < missionObjChars.Length)
        {
            ObjectiveTxt.text += missionObjChars[charCounter];            
            charCounter++;
            StartCoroutine(AddLettersToObjective());
        }        
    }

    IEnumerator PlayBlipSound()
    {
        yield return new WaitForSeconds(blipSoundSpeed);
        TheGame.theGameInst.audioManager.PlaySound("Blip_02");
        if(charCounter < missionObjChars.Length)
        {
            StartCoroutine(PlayBlipSound());
        }
    }
}
