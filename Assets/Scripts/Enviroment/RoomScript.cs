using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public Transform playerSpawnPoint;

    [Button]
    public void MoveSpawnPointToPlayer()
    {
        //FIND PLAYER
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("No player found");
            return;
        }

        playerSpawnPoint.position = player.transform.position;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("BACKROOMS");
            PlayerController.Instance.transform.position = playerSpawnPoint.position;
        }
    }
}