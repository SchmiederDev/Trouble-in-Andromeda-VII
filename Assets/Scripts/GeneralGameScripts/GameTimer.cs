using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public int seconds { get; private set; } = 0;
    public int minutes { get; private set; } = 0;

    public void Init_Timer()
    {
        StartCoroutine(WaitForMissionStart());
    }

    IEnumerator WaitForMissionStart()
    {
        yield return new WaitForSeconds(0.01f);

        if (!TheGame.theGameInst.MissionCanBegin)
            StartCoroutine(WaitForMissionStart());
        else
            StartCoroutine(Clock());
    }

    IEnumerator Clock()
    {
        yield return new WaitForSeconds(1f);

        if(seconds < 60)
        {
            seconds++;
        }

        else
        {
            if(minutes < 60)
            {
                seconds = 0;
                minutes++;
            }

            else
            {
                seconds = 0;
                minutes = 0;
            }

        }

        StartCoroutine(Clock());
    }

    public void Reset_Timer()
    {
        seconds = 0;
        minutes = 0;
    }
}
