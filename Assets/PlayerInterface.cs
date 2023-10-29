using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PlayerInterface : MonoBehaviour
{
    public AudioMixer audioMixer;
    
    public Color dangerColor;
    public Color okColor;
    public Color deathColor;
    public Image dudeFace;

    public Sprite dudeFaceOk;
    public Sprite dudeFaceScared;
    
    public TextMeshProUGUI ammoText;

    private float gameTimeScale;
    public GameObject pauseMenu;

    public Slider sfx;
    public Slider music;
    
    private bool isPaused = false;

    private void Start()
    {
        sfx.value = PlayerPrefs.GetFloat("SFX", 0.8f);
        music.value = PlayerPrefs.GetFloat("Music", 0.8f);
        
        UpdateMusicVolume(music);
        UpdateSfxVolume(sfx);
        
        pauseMenu.SetActive(isPaused);
    }

    private void Update()
    {
        if (PlayerController.Instance.lives == 1)
        {
            dudeFace.color = okColor;
            dudeFace.sprite = dudeFaceOk;
        }
        else if (PlayerController.Instance.lives == 0)
        {
            dudeFace.color = dangerColor;
            dudeFace.sprite = dudeFaceScared;
        }
        else
        {
            dudeFace.color = deathColor;
            dudeFace.sprite = dudeFaceScared;
        }

        if (!PistolController.Instance.isEquipped)
        {
            ammoText.text = "";
            return;
        }

        if (PistolController.Instance.isLoaded)
        {
            ammoText.text = "|";
        }
        else
        {
            ammoText.text = "0";
        }
    }

    public void Pause()
    {
        Debug.Log("Pause");
        if (isPaused)
        {
            UnPause();
            return;
        }
        isPaused = true;
        gameTimeScale = Time.timeScale;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        
    }

    public void UnPause()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = gameTimeScale;
    }
    
    public void Restart()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        GameManager.Instance.GameOver();
    }
    
    
    public void UpdateSfxVolume(Slider slider)
    {
        float volume = Mathf.Lerp(-80f, 0f, slider.value);
        audioMixer.SetFloat("SFX", volume);
    }

    public void UpdateMusicVolume(Slider slider)
    {
        float volume = Mathf.Lerp(-80f, 0f, slider.value);
        audioMixer.SetFloat("Music", volume);
    }


}
