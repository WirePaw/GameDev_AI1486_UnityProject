using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Can_count_down_timer : MonoBehaviour
{
    public float waitTime;
    public float runTime;
    private bool isRunning, hasFinished;

    void Start()
    {
        resetTimer();
    }

    void FixedUpdate()
    {
        if (isRunning)
        {
            if (runTime >= waitTime) // timer has finished waiting
            {
                isRunning = false;
                hasFinished = true;
            }
            else
            {
                runTime += Time.fixedDeltaTime;
            }
        }
    }

    public void startTimer()
    {
        if(!isRunning && !hasFinished)
        {
            isRunning = true;
            hasFinished = false;
        }
    }

    public void resetTimer()
    {
        isRunning=false;
        hasFinished = false;
        runTime = 0.0f;
    }

    //add pause function?

    public bool hasStarted()
    {
        return isRunning;
    }
    public bool hasEnded()
    {
        return hasFinished;
    }
}
