using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ObjectiveStarter : MonoBehaviour
{
    [SerializeField]
    private Image MissionObjectiveImg;
    
    float ObjectiveImgAlpha = 0;

    [SerializeField]
    private ObjectiveText MissionObjectiveTxt;

    string[] KeyBordSettingMessages = { "Press Arrow Keys to Maneuver < >", "Hit Space to Fire!", "Press Esc to Enter Menu" };
    int keyboardSettingIndex = 0;

    [SerializeField]
    private TMP_Text SkipText;

    [SerializeField]
    float fadeInRate = 0.025f;
    [SerializeField]
    float fadeOutRate = 0.01f;

    [SerializeField]
    float fadeInTimeSpan = 0.1f;
    [SerializeField]
    float fadeOutTimeSpan = 0.2f;

    bool isFading = true;

    [SerializeField]
    private float KeyBoardSettingStartTime = 1f;

    [SerializeField]
    private float KeyBoardSettingMessageTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        MissionObjectiveImg = GetComponent<Image>();
        MissionObjectiveTxt = GetComponentInChildren<ObjectiveText>();
        StartCoroutine(Fade());
        Start_MissionObjective();
    }

    private void Update()
    {
        if(isFading)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ExitObjectiveStarter();
                BeginMission();
                ShowKeyBoardSettings();
            }
        }
    }

    public void StartFade()
    {
        isFading = true;
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
            ShowKeyBoardSettings();
            StartCoroutine(FadeOut());
        }
        
    }

    private void FadeIn()
    {
        ObjectiveImgAlpha += fadeInRate;
        MissionObjectiveImg.color = new Color(MissionObjectiveImg.color.r, MissionObjectiveImg.color.g, MissionObjectiveImg.color.b, ObjectiveImgAlpha);
        SkipText.color = new Color(SkipText.color.r, SkipText.color.g, SkipText.color.b, ObjectiveImgAlpha);
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(fadeOutTimeSpan);
        
        if(ObjectiveImgAlpha > 0)
        {
            ObjectiveImgAlpha -= fadeOutRate;
            MissionObjectiveImg.color = new Color(MissionObjectiveImg.color.r, MissionObjectiveImg.color.g, MissionObjectiveImg.color.b, ObjectiveImgAlpha);
            SkipText.color = new Color(SkipText.color.r, SkipText.color.g, SkipText.color.b, ObjectiveImgAlpha);
            StartCoroutine(FadeOut());
        }
        else
        {
            isFading = false;
            BeginMission();
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

    private void BeginMission()
    {
        TheGame.theGameInst.MissionCanBegin = true;
        TheGame.theGameInst.onMissionCanBegin.Invoke();        

        if (TheGame.theGameInst.ActiveLevel.UnlockLevelAccomplishment)
        {
            TheGame.theGameInst.Set_FlashMessage(TheGame.theGameInst.ActiveLevel.UnlockText);
            TheGame.theGameInst.PlayerUnionFighter.ActivateWeapon(TheGame.theGameInst.ActiveLevel.UnlockIndex);
        }
    }

    private void ExitObjectiveStarter()
    {
        StopCoroutines();
        HideObjectiveStarter();        
    }

    private void StopCoroutines()
    {
        StopAllCoroutines();
        MissionObjectiveTxt.StopAllCoroutines();
    }

    private void HideObjectiveStarter()
    {
        ObjectiveImgAlpha = 0;
        MissionObjectiveImg.color = new Color(MissionObjectiveImg.color.r, MissionObjectiveImg.color.g, MissionObjectiveImg.color.b, ObjectiveImgAlpha);
        SkipText.color = new Color(SkipText.color.r, SkipText.color.g, SkipText.color.b, ObjectiveImgAlpha);
        MissionObjectiveTxt.HideText();
    }

    private void ShowKeyBoardSettings()
    {
        Scene CurrentScene = SceneManager.GetActiveScene();

        if (CurrentScene.buildIndex == 0)
            StartCoroutine(PlayKeyboardSettingOnStart());
    }

    private IEnumerator PlayKeyboardSettingOnStart()
    {
        yield return new WaitForSecondsRealtime(KeyBoardSettingStartTime);

        StartCoroutine(PlayKeyboardSetting());
    }

    private IEnumerator PlayKeyboardSetting()
    {
        yield return new WaitForSecondsRealtime(KeyBoardSettingMessageTime);

        if (keyboardSettingIndex < KeyBordSettingMessages.Length)
        {
            TheGame.theGameInst.Set_FlashMessage(KeyBordSettingMessages[keyboardSettingIndex]);
            keyboardSettingIndex++;

            StartCoroutine(PlayKeyboardSetting());
        }

        else
            TheGame.theGameInst.Set_FlashMessage("");
    }
}
