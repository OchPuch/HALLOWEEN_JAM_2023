using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PistolController : MonoBehaviour
{
    public static PistolController Instance;
    private Camera _mainCamera;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public GameObject pistolCenter;
    public bool isLoaded = true;
    public bool isEquipped = false;
    public PistolPropPhysics pistolPropPrefab;
    public float throwForce = 10f;
    public GameObject loadedIndicator;
    public Transform pistolThrowPoint;
    

    private void Awake()
    {
        _mainCamera = Camera.main;
        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (GameManager.Instance.loadedFromCheckpoint)
        {
            isEquipped = GameManager.Instance.gunIsEquippedOnCheckpoint;
            if (isEquipped) isLoaded = GameManager.Instance.gunIsLoadedOnCheckpoint;
            return;
        }
        isEquipped = GameManager.Instance.gunIsEquippedOnLeave;
        if (isEquipped) isLoaded = GameManager.Instance.gunIsLoadedOnLeave;
    }
    
    private void Start()
    {
        pistolCenter.SetActive(isEquipped);
    }
    
    private void Update()
    {
        loadedIndicator.SetActive(isLoaded);
        pistolCenter.SetActive(isEquipped);
        
        if (!isEquipped) return;
        if (PlayerController.Instance.currentState == PlayerController.Instance.deathState) return;
        
        Vector3 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        pistolCenter.transform.rotation = Quaternion.Euler(0f,0f,angle);
        if (Input.GetMouseButtonDown(0) && isLoaded)
        {
            Shoot();
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            ThrowPistol();
        }
    }
    
    private void Shoot()
    {
        PlayerAudio.Instance.PlayShootSound();
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, pistolCenter.transform.rotation);
        isLoaded = false;
    }
    
    public void PickUpPistol(PistolProp pistolProp)
    {
        PlayerAudio.Instance.PlayPickUpSound();
        isEquipped = true;
        isLoaded = pistolProp.isLoaded;
        
    }
    
    public void ThrowPistol()
    {
        PlayerAudio.Instance.PlayThrowSound();
        var pistolPropPhysics = Instantiate(pistolPropPrefab, pistolThrowPoint.position, pistolCenter.transform.rotation);
        pistolPropPhysics.isThrown = true;
        var pistolProp = pistolPropPhysics.connectedProp;
        pistolProp.isLoaded = isLoaded;
        pistolProp.physics.rb.velocity = PlayerController.Instance.rb.velocity;
        pistolProp.physics.rb.AddForce(pistolCenter.transform.right * throwForce, ForceMode2D.Impulse);
        
        isEquipped = false;
    }
}
