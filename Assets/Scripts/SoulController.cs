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
    [Header("Soul settings")]
    public Vector3 soulDestination;
    public bool isActivated = false;
    public SoulMovement soulMovement;
    public float flyDistance = 1f;
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
        CalculateSoulPosition(_mainCamera.ScreenToWorldPoint(Input.mousePosition));
        if (isActivated) soulMovement.head.position = soulDestination;
    }
    
    public void ActivateSoul()
    {
        soulMovement.ActivateSoul();
    }
    
    [Button]
    public void DeactivateSoul()
    {
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
