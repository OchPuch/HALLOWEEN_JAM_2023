using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class SoulController : MonoBehaviour
{
    [Header("General")]
    public PlayerController playerController;
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
    
    public void Start()
    {
        _mainCamera = Camera.main;
        playerController.OnStateChange += OnStateChange;
    }
    
    private void OnStateChange(PlayerState newState)
    {
        if (newState == playerController.deathState)
        {
            ActivateSoul();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && isActivated)
        {
            DeactivateSoul();
        }
        if (Input.GetMouseButtonDown(0) && !isActivated)
        {
            playerController.Die();
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
