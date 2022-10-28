using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveStarter : MonoBehaviour
{
    [SerializeField]
    private Image MissionObjectiveImg;

    [SerializeField]
    private ObjectiveText MissionObjectiveTxt;
    
    float ObjectiveImgAlpha = 0;

    [SerializeField]
    float fadeInRate = 0.025f;
    [SerializeField]
    float fadeOutRate = 0.01f;

    [SerializeField]
    float fadeInTimeSpan = 0.1f;
    [SerializeField]
    float fadeOutTimeSpan = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        MissionObjectiveImg = GetComponent<Image>();
        MissionObjectiveTxt = GetComponentInChildren<ObjectiveText>();
        StartCoroutine(Fade());
        Start_MissionObjective();
    }

    public void StartFade()
    {
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        yield return new WaitForSeconds(fadeInTimeSpan);

        if (ObjectiveImgAlpha <= 1f)
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
        ObjectiveImgAlpha += fadeInRate;
        MissionObjectiveImg.color = new Color(MissionObjectiveImg.color.r, MissionObjectiveImg.color.g, MissionObjectiveImg.color.b, ObjectiveImgAlpha);            
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(fadeOutTimeSpan);
        
        if(ObjectiveImgAlpha > 0)
        {
            ObjectiveImgAlpha -= fadeOutRate;
            MissionObjectiveImg.color = new Color(MissionObjectiveImg.color.r, MissionObjectiveImg.color.g, MissionObjectiveImg.color.b, ObjectiveImgAlpha);
            StartCoroutine(FadeOut());
        }
        else
        {
            TheGame.theGameInst.MissionCanBegin = true;
            TheGame.theGameInst.onMissionCanBegin.Invoke();

            if(TheGame.theGameInst.ActiveLevel.UnlockLevelAccomplishment)
            {
                TheGame.theGameInst.Set_FlashMessage(TheGame.theGameInst.ActiveLevel.UnlockText);
                TheGame.theGameInst.PlayerUnionFighter.ActivateWeapon(TheGame.theGameInst.ActiveLevel.UnlockIndex);
            }
        }
    }

    public void Start_MissionObjective()
    {
        MissionObjectiveTxt.Init_MissionObjective();
        MissionObjectiveTxt.Play_MissionObjective();
    }

    public void StopCoroutines_InObjectiveTxtChild()
    {
        MissionObjectiveTxt.StopAllCoroutines();
    }

}
