using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalClip : MonoBehaviour
{
    public AudioSource audioSource;
    
    public void Start()
    {
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        float time = audioSource.clip.length;
        Debug.Log(time);
        yield return new WaitForSecondsRealtime(time);
        Debug.Log("Game Over");
        Application.Quit();
    }
}
