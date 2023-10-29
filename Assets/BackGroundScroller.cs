using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundScroller : MonoBehaviour
{
    public float scrollSpeed;
    public RawImage rawImage;
    void Update()
    {
        rawImage.uvRect = new Rect(rawImage.uvRect.x + scrollSpeed * Time.deltaTime, 0f, 1f, 1f);
    }
}
