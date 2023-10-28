using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


public class SoulMovement : MonoBehaviour
{
    [Header("General")]
    public Rigidbody2D rbHead;
    public Transform head;
    public Transform player;
    public SoulController soulController;
    public SoulState soulState = SoulState.Waiting;
    [Header("Soul points")]
    public List<Transform> soulPoints;
    private Dictionary<Transform, float> _soulPointsDistances = new Dictionary<Transform, float>();
    [Header("Despawn Animation")] 
    public float despawnTime = 5f;
    public float despawnAcceleration = 1f;
    [Header("Particles")]
    public GameObject spawnParticles;
    public GameObject despawnParticles;

    public enum SoulState
    {
        Waiting,
        Starting,
        Moving,
        Ending
    }
    


    private void Start()
    {
        //init 
        foreach (var soulPoint in soulPoints)
        {
            _soulPointsDistances.Add(soulPoint,(float) soulPoints.IndexOf(soulPoint) / soulPoints.Count);
            soulPoint.gameObject.SetActive(false);
        }
        head.gameObject.SetActive(false);
    }

    public void ActivateSoul()
    {
        soulState = SoulState.Starting;
        head.gameObject.SetActive(true);
        foreach (var soulPoint in soulPoints)
        {
            soulPoint.gameObject.SetActive(true);
            soulPoint.position = soulController.transform.position;
        }

        StartCoroutine(SpawnHeadAnimation());
    }
    
    public void DeactivateSoul()
    {
        soulState = SoulState.Ending;
        StartCoroutine(DespawnHeadAnimation());
    }

    private void Update()
    {
        float distance = Vector2.Distance(soulController.transform.position, head.position);
        Vector2 direction = (head.position - soulController.transform.position).normalized;
        //calculate soul points 
        UpdateForcesForSoulPoints(distance, direction);  
    }

    public void UpdateForcesForSoulPoints(float distance, Vector2 direction)
    {
        foreach (var soulPoint in soulPoints)
        {
            var distanceOnLine = Mathf.Lerp(0, distance, _soulPointsDistances[soulPoint]);
            //destination point
            var destinationPoint = soulController.transform.position + (Vector3) direction * distanceOnLine;
            soulPoint.position = destinationPoint;

        }
    }

    private IEnumerator SpawnHeadAnimation()
    {
        //animation
        //Instantiate(spawnParticles, head.position, Quaternion.identity);
        head.position = soulController.transform.position;
        
        
        //logic
        rbHead.velocity = Vector3.zero;
        soulState = SoulState.Moving;
        yield break;
    }
    
    private IEnumerator DespawnHeadAnimation()
    {
        rbHead.velocity = Vector3.zero;
        float estimatedTime = Vector3.Distance(soulController.transform.position, head.position) /
                              soulController.flyDistance * despawnTime;
        float acceleration = 1;
        Vector2 headStartPos = head.position;
        //animation
        while (estimatedTime > 0)
        {
            estimatedTime -= Time.unscaledDeltaTime * acceleration;
            acceleration += despawnAcceleration * Time.unscaledDeltaTime;
            head.position = Vector3.Lerp(headStartPos, soulController.transform.position, (despawnTime - estimatedTime)/despawnTime);
            yield return null;
        }
       
        //Instantiate(despawnParticles, head.position, Quaternion.identity);
        
        //logic
        rbHead.velocity = Vector3.zero;
        head.gameObject.SetActive(false);
        foreach (var soulPoint in soulPoints)
        {
            soulPoint.gameObject.SetActive(false);
        }
        soulState = SoulState.Waiting;
        
        //revive player
        soulController.playerController.Revive();
    }
    
    
}
