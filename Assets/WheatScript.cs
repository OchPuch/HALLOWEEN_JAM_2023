using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheatScript : MonoBehaviour
{
    public RawImage rawImage;
    public float scrollSpeed;
    
    void Update()
    {
        rawImage.uvRect = new Rect(rawImage.uvRect.x + PlayerController.Instance.rb.velocityX * scrollSpeed * Time.deltaTime, 0f, 1f, 1f);
    }
}
