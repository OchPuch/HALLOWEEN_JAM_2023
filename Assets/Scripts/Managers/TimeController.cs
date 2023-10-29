using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance;
    public float baseTimeScale = 1;
    public float slowDownTimeScale = 0.05f;
    public float baseUnscaledChangeTime = 0.5f;

    private void Awake()
    {
        Instance = this;
    }

    public void SlowDownTime()
    {
        SlowDownTime(baseUnscaledChangeTime,slowDownTimeScale);
    }
    
    public void SpeedUpTime()
    {
        SpeedUpTime(baseUnscaledChangeTime,baseTimeScale);
    }

    public void SlowDownTime(float estimatedTime, float newTimeScale)
    {
        StartCoroutine(SlowDownTimeCoroutine(estimatedTime, newTimeScale));
    }
    
    private IEnumerator SlowDownTimeCoroutine(float estimatedTime, float newTimeScale)
    {
        float time = 0;
        float oldTimeScale = Time.timeScale;
        while (time < estimatedTime)
        {
            time += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(oldTimeScale, newTimeScale, time / estimatedTime);
            yield return null;
        }
        Time.timeScale = newTimeScale;
    }
    
    public void SpeedUpTime(float estimatedTime, float newTimeScale)
    {
        StartCoroutine(SpeedUpTimeCoroutine(estimatedTime, newTimeScale));
    }
    
    private IEnumerator SpeedUpTimeCoroutine(float estimatedTime, float newTimeScale)
    {
        float time = 0;
        float oldTimeScale = Time.timeScale;
        while (time < estimatedTime)
        {
            time += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(oldTimeScale, newTimeScale, time / estimatedTime);
            yield return null;
        }
        Time.timeScale = newTimeScale;
    }
}
