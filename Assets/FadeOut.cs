using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public float fadeTime = 1f;
    public Image fadeImage;
    
    public void Start()
    {
        StartCoroutine(FadeOutImage());
    }
    
    private IEnumerator FadeOutImage()
    {
        var elapsedTime = 0f;
        var startColor = fadeImage.color;
        var endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        while (elapsedTime < fadeTime)
        {
            fadeImage.color = Color.Lerp(startColor, endColor, elapsedTime / fadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = endColor;
        fadeImage.gameObject.SetActive(false);
    }
}
