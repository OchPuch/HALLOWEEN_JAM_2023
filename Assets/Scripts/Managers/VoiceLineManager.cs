using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLineManager : MonoBehaviour
{
    public static VoiceLineManager Instance;
    public AudioSource audioSource;
    
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        GameManager.Instance.OnGameOver += StopAudio;
    }
    
    public void PlayVoiceLine(AudioClip clip)
    {
        if (PlayerPrefs.GetInt(clip.name, 0) == 1) return;
        StopAllCoroutines();
        audioSource.clip = clip;
        audioSource.Play();
        StartCoroutine(WaitForClipToEnd());
    }
    
    private void StopAudio()
    {
        audioSource.Stop();
        StopAllCoroutines();
    }
    
    private IEnumerator WaitForClipToEnd()
    {
        yield return new WaitForSecondsRealtime(audioSource.clip.length);
        //Setplayer prefs what clip was played
        PlayerPrefs.SetInt(audioSource.clip.name, 1);
    }
}
