using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CanCountdownTimer : MonoBehaviour
{
    //attributes
    public float maxTime;
    public float currentTime;
    public bool hasFinished;

    //references

    //methods (actions?)
    public void StartTimer()
    {
        hasFinished = false;
        StartCoroutine(RunTimer());
    }

    public bool HasFinished()
    {
        return hasFinished;
    }

    public IEnumerator RunTimer()
    {
        currentTime = 0;
        while (currentTime < maxTime)
        {
            currentTime += Time.fixedDeltaTime;
            if(currentTime >= maxTime)
            {
                hasFinished = true;
            }
            yield return new WaitUntil(() => Time.timeScale > 0);
        }
    }
}
