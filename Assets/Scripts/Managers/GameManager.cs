using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Vector2 playerVelocityOnLeave = Vector2.zero;
    public bool gunIsLoadedOnLeave = false;
    public bool gunIsEquippedOnLeave = false;
    public int livesOnLeave = 1;

    public float waitAfterGameOverTime = 3f;
    private bool loadingCheckpoint = false;
    
    public bool gunIsLoadedOnCheckpoint = false;
    public bool gunIsEquippedOnCheckpoint = false;
    public int livesOnCheckpoint = 1;
    
    public bool loadedFromCheckpoint = false;

    public int lastCheckpointSceneIndex;  // Индекс сцены с последним чекпоинтом
    
    public Action OnGameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        // Получение индекса сцены с последним чекпоинтом из PlayerPrefs
        lastCheckpointSceneIndex = PlayerPrefs.GetInt("LastCheckpointSceneIndex", 0);
        livesOnCheckpoint = PlayerPrefs.GetInt("LivesOnCheckPoint", 1);
        gunIsEquippedOnCheckpoint = PlayerPrefs.GetInt("GunEquiped", 0) == 1;
        gunIsLoadedOnCheckpoint = PlayerPrefs.GetInt("GunLoaded", 0) == 1;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(UndoingCheckpoint());
    }

    private IEnumerator UndoingCheckpoint()
    {
        yield return new WaitForSecondsRealtime(1f);
        loadingCheckpoint = false;
        loadedFromCheckpoint = false;
    }
    
    public void NextLevel()
    {
        playerVelocityOnLeave = PlayerController.Instance.rb.velocity;
        gunIsLoadedOnLeave = PistolController.Instance.isLoaded;
        gunIsEquippedOnLeave = PistolController.Instance.isEquipped;
        livesOnLeave = PlayerController.Instance.lives;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No more scenes to load.");
        }
    }
    
[Button]
    public void SetCheckpoint()
    {
        // Сохранение индекса текущей сцены как сцены с последним чекпоинтом
        lastCheckpointSceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("LastCheckpointSceneIndex", lastCheckpointSceneIndex);
        gunIsLoadedOnCheckpoint = PistolController.Instance.isLoaded;
        PlayerPrefs.SetInt("GunLoaded", gunIsLoadedOnCheckpoint ? 1 : 0);
        gunIsEquippedOnCheckpoint = PistolController.Instance.isEquipped;
        PlayerPrefs.SetInt("GunEquiped", gunIsEquippedOnCheckpoint ? 1 : 0);
        livesOnCheckpoint = PlayerController.Instance.lives;
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        //LOAD LAST CHECKPOINT
        loadedFromCheckpoint = true;
        if (!loadingCheckpoint)
        {
            loadingCheckpoint = true;
            StartCoroutine(WaitAfterGameOver());
        }
    }

    public IEnumerator WaitAfterGameOver()
    {
        yield return new WaitForSecondsRealtime(waitAfterGameOverTime);
        OnGameOver?.Invoke();
        SceneManager.LoadScene(lastCheckpointSceneIndex);
    }
}
