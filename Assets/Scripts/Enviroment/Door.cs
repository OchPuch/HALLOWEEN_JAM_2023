using System;
using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isLocked = true;
    public GameObject doorSprite;
    public SpriteRenderer doorLight;
    public Color closedColor;
    public Color openColor;
    public Transform doorOpenPoint;
    public float doorOpenTime = 1f;
    public void SetActive(bool active)
    {
        isLocked = !active;
        doorLight.color = active ? openColor : closedColor;
        if (active)
        {
            StopAllCoroutines();
            StartCoroutine(OpenDoor());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(CloseDoor());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isLocked)
            {
                return;
            }
            GameManager.Instance.NextLevel();
        }
    }

    public IEnumerator OpenDoor()
    {
        var startPos = doorSprite.transform.position;
        var endPos = doorOpenPoint.position;
        float elapsedTime = 0;
        while (elapsedTime < doorOpenTime)
        {
            doorSprite.transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / doorOpenTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        doorSprite.transform.position = endPos;
    }
    
    public IEnumerator CloseDoor()
    {
        var startPos = doorSprite.transform.position;
        var endPos = transform.position;
        float elapsedTime = 0;
        while (elapsedTime < doorOpenTime)
        {
            doorSprite.transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / doorOpenTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        doorSprite.transform.position = endPos;
    }
}