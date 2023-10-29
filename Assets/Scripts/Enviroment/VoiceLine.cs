using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLine : MonoBehaviour
{
    public AudioClip clip;

    private float clipLength;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            VoiceLineManager.Instance.PlayVoiceLine(clip);
            Destroy(this);
        }
    }
}
