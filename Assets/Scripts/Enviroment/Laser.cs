using System;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject ray;
    public bool isActivated = true;

    private void Start()
    {
        ray.SetActive(isActivated);
    }

    public void SetActive(bool active)
    {
        isActivated = active;
        ray.SetActive(isActivated);
        
    }
}