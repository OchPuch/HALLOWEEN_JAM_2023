using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class SoulController : MonoBehaviour
{
    public static SoulController Instance;
    [Header("General")]
    public PlayerController playerController;
    public SoulHead soulHead;
    private Camera _mainCamera;
    public bool canInteract;
    [Header("Soul settings")]
    public float soulPower = 1f;
    public Vector3 soulDestination;
    public bool isActivated = false;
    public SoulMovement soulMovement;
    public float flyDistance = 1f;
    public Vector2 mouseOffset;
    private Vector3 _mousePos;
    [Header("Deactivation")]
    public float deactivateDistance;
    public Vector3 deactivateDirection;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        _mainCamera = Camera.main;
        playerController.OnStateChange += OnStateChange;
    }
    
    private void OnStateChange(PlayerState newState)
    {
        if (newState == playerController.deathState)
        {
            if (playerController.lives < 0) return;
            ActivateSoul();
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0) return;

        if (playerController.currentState != playerController.deathState) return;
        bool isMouseOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
        if (Input.GetMouseButtonDown(0) && isActivated && !isMouseOverUI)
        {
            DeactivateSoul();
        }

        if (Input.GetMouseButtonDown(1) && canInteract && !isMouseOverUI) 
        {
            soulHead.closestThing.Interact();
        }
        
        CalculateSoulPosition(_mainCamera.ScreenToWorldPoint(Input.mousePosition + (Vector3) mouseOffset));
        if (isActivated) soulMovement.head.position = soulDestination;
    }
    
    
    public void ActivateSoul()
    {
        TimeController.Instance.SlowDownTime();
        mouseOffset = playerController.transform.position - _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        soulMovement.ActivateSoul();
        isActivated = true;
    }
    
    public void DeactivateSoul()
    {
        TimeController.Instance.SpeedUpTime();
        deactivateDistance = Vector3.Distance(soulMovement.head.position, playerController.transform.position);
        deactivateDirection = (-soulMovement.head.position + playerController.transform.position).normalized;
        soulMovement.DeactivateSoul();
        isActivated = false;
    }
    
    public void CalculateSoulPosition(Vector2 mousePos)
    {
        Vector2 distance = (Vector3) mousePos - playerController.transform.position;
        Vector2 direction = distance.normalized;
        Vector2 soulPos = distance.magnitude > flyDistance ? playerController.transform.position + (Vector3) direction * flyDistance : mousePos;
        soulDestination = soulPos;
    }
}
