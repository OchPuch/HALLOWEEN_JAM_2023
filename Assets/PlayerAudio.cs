using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public static PlayerAudio Instance;
    public AudioSource movingSounds;
    public AudioSource liveSounds;
    public AudioSource gunSounds;
    
    public AudioClip[] stepClips;
    public AudioClip jumpSound;
    public AudioClip reviveSound;
    public AudioClip deathSound;
    public AudioClip shootSound;
    public AudioClip pickUpSound;
    public AudioClip throwSound;
    
    private bool playingStepSound = false;
    
    private void Awake()
    {
        Instance = this;
    }
    
    public void PlayStepSound()
    {
        if (playingStepSound) return;
        playingStepSound = true;
        StartCoroutine(StepSoundCoroutine());
    }
    
    private IEnumerator StepSoundCoroutine()
    {
        while (true)
        {
            if (!(CustomPlayerAnimator.Instance.animator.GetBool("IsRunning")))
            {
                playingStepSound = false;
                movingSounds.pitch = 1;
                yield break;
            }
            movingSounds.clip = stepClips[UnityEngine.Random.Range(0, stepClips.Length)];
            //ranndom pitch
            movingSounds.pitch = UnityEngine.Random.Range(0.85f, 1f);
            movingSounds.Play();
            yield return new WaitForSeconds(movingSounds.clip.length);
        }
    }
    
    public void PlayJumpSound()
    {
        movingSounds.clip = jumpSound;
        movingSounds.Play();
    }
    
    public void PlayReviveSound()
    {
        liveSounds.clip = reviveSound;
        liveSounds.Play();
    }
    
    public void PlayDeathSound()
    {
        liveSounds.clip = deathSound;
        liveSounds.Play();
    }
    
    public void PlayShootSound()
    {
        gunSounds.clip = shootSound;
        gunSounds.Play();
    }
    
    public void PlayPickUpSound()
    {
        gunSounds.clip = pickUpSound;
        gunSounds.Play();
    }
    
    public void PlayThrowSound()
    {
        gunSounds.clip = throwSound;
        gunSounds.Play();
    }
    
    
    
    
}
