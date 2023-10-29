using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public AudioSource musicSource;
    private int _currentMusicIndex = 0; 
    public int musicClipsLength = 3;
    [Header("Starting")]
    public bool startingSlow = true;
    [Header("Playing")]
    public MusicState musicState = MusicState.StartingSlow;
    [Header("Slow")] 
    public AudioClip[] slowMusicIntro = new AudioClip[3];
    public AudioClip[] slowMusicLoop = new AudioClip[3];
    [Header("Fast")]
    public AudioClip[] fastMusicIntro = new AudioClip[3];
    public AudioClip[] fastMusicLoop = new AudioClip[3];

    public AudioClip finalClip;
    public bool playingFinal = false;

    public enum MusicState
    {
        StartingSlow,
        LoopSlow,
        StartingFast,
        LoopFast,
        FinalClip
        
    }

   

    public void ChangeToFast()
    {
        if (playingFinal) return;
        //If its currently playing slow intro, then its going to finish the whole intro (all 3 clips) and then play the fast loop
        //if its currently playing a slow loop, then its going to finish the current loop (single audio clip) and then play the fast loop
        //if its currently playing a fast intro, then its going to finish the whole intro (all 3 clips) and then play the fast loop 
        musicState = MusicState.LoopFast;

    }
    
    public void ChangeToSlow()
    {
        if (playingFinal) return;
        //if its currently playing fast intro, then its going to finish the whole intro (all 3 clips) and then play the slow loop
        //if its currently playing a fast loop, then its going to finish the current loop (single audio clip) and then play the slow loop
        //if its currently playing a slow intro, then its going to finish the whole intro (all 3 clips) and then play the slow loop
        musicState = MusicState.LoopSlow;
    }

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

    }
    
    public void Start()
    {
        musicState = startingSlow ? MusicState.StartingSlow : MusicState.StartingFast;
        if (playingFinal)
        {
            musicState = MusicState.FinalClip;
            return;
        }
        StartCoroutine(PlayWholeIntro(musicState));
    }

    public IEnumerator PlayWholeIntro(MusicState introState)
    {
        if (playingFinal) yield break;
        if (introState == MusicState.StartingSlow)
        {
            if (playingFinal) yield break;
            for (int i = 0; i < slowMusicIntro.Length; i++)
            {
                musicSource.clip = slowMusicIntro[i];
                if (playingFinal) yield break;
                musicSource.Play();
                musicSource.loop = false;
                yield return new WaitForSecondsRealtime(musicSource.clip.length);
            }
            if (musicState == MusicState.StartingSlow) musicState = MusicState.LoopSlow;
        }
        else if (introState == MusicState.StartingFast)
        {
            if (playingFinal) yield break;
            for (int i = 0; i < fastMusicIntro.Length; i++)
            {
                musicSource.clip = fastMusicIntro[i];
                if (playingFinal) yield break;
                musicSource.Play();
                musicSource.loop = false;
                yield return new WaitForSecondsRealtime(musicSource.clip.length);
            }
            if (musicState == MusicState.StartingFast) musicState = MusicState.LoopFast;
        }
        
        if (playingFinal) yield break;
        StartCoroutine(PlayLoop());
    }
    
    public IEnumerator PlayLoop()
    {
        while (true)
        {
if (playingFinal) yield break;
            for (int i = 0; i < musicClipsLength; i++)
            {
                if (playingFinal) yield break;
                switch (musicState)
                {
                    case MusicState.LoopSlow:
                        if (playingFinal) yield break;
                        musicSource.clip = slowMusicLoop[i];
                        break;
                    case MusicState.LoopFast:
                        if (playingFinal) yield break;
                        musicSource.clip = fastMusicLoop[i];
                        break;
                    case MusicState.StartingSlow:
                        if (playingFinal) yield break;
                        musicSource.clip = slowMusicLoop[i];
                        break;
                    case MusicState.StartingFast:
                        if (playingFinal) yield break;
                        musicSource.clip = fastMusicLoop[i];
                        break;
                }
                if (playingFinal) yield break;
                musicSource.Play();
                musicSource.loop = false;
                yield return new WaitForSecondsRealtime(musicSource.clip.length);
            }
        }
    }

    public void ChangeToFinal()
    {
        Destroy(gameObject);
    }

  
   
}
