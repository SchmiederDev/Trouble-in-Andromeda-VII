using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimerUI : MonoBehaviour
{
    public GameObject MinutesEmpty;
    public GameObject SecondsEmpty;

    public TMP_Text Minutes_Txt;
    public TMP_Text Seconds_Txt;

    int minutes;
    int seconds;

    // Start is called before the first frame update
    void Start()
    {
        MinutesEmpty = GameObject.FindGameObjectWithTag("MinuteTimerTxt");
        SecondsEmpty = GameObject.FindGameObjectWithTag("SecondTimerTxt");
        Minutes_Txt = MinutesEmpty.GetComponent<TMP_Text>();
        Seconds_Txt = SecondsEmpty.GetComponent<TMP_Text>();
    }

    private void Update()
    {
        seconds = TheGame.theGameInst.Timer.seconds;
        minutes = TheGame.theGameInst.Timer.minutes;
    }

    private void FixedUpdate()
    {
        ShowTime();
    }

    private void ShowTime()
    {
        if (seconds != 60)
        {
            if(seconds < 10)
                Seconds_Txt.text = "0" + seconds.ToString();
            else
                Seconds_Txt.text = seconds.ToString();
        }

        if (minutes != 60)
        {
            if(minutes < 10)
                Minutes_Txt.text = "0" + minutes.ToString();
            else
                Minutes_Txt.text = minutes.ToString();
        }        
        
    }
}
