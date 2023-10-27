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
    [Header("Soul points animation")]
    public List<Transform> soulPoints;
    public float maxSpeedPoint = 1f;
    public float maxForcePoint = 1f;
    private Dictionary<Transform, PidRegulation> _soulPointsPids = new Dictionary<Transform, PidRegulation>();
    private Dictionary<Transform, float> _soulPointsDistances = new Dictionary<Transform, float>();
    private Dictionary<Transform, Rigidbody2D> _soulPointsRbs = new Dictionary<Transform, Rigidbody2D>();
    [Header("Spawn Animation")]
    public PidRegulation spawnPid;
    public float maxSpeed = 1f;
    public float maxForce = 1f;
    public float errorThreshold = 0.1f;
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
    
    [Button]
    public void TestActivation()
    {
        ActivateSoul();
    }
    
    [Button]
    public void TestDeactivation()
    {
        soulController.DeactivateSoul();
        
    }

    private void Start()
    {
        //init 
        foreach (var soulPoint in soulPoints)
        {
            //create new pid for each soul point
            //set its parameters to soulController spawnPid
            //add it to _soulPointsPids
            PidRegulation pid = new PidRegulation(spawnPid.kp, spawnPid.ki, spawnPid.kd);
            _soulPointsPids.Add(soulPoint, pid);
            //add soul point to _soulPointsDistances
            _soulPointsDistances.Add(soulPoint,(float) soulPoints.IndexOf(soulPoint) / soulPoints.Count);
            //add soul point to _soulPointsRbs
            _soulPointsRbs.Add(soulPoint, soulPoint.GetComponent<Rigidbody2D>());
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
            //reset pid errors
            _soulPointsPids[soulPoint].Reset();
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
            //distance to destination point
            Vector2 distanceToDestinationPoint = (destinationPoint - soulPoint.position);
            //direction to destination point
            Vector2 directionToDestinationPoint = distanceToDestinationPoint.normalized;
            //calculate error with sign
            
            //calculate force
            var force = _soulPointsPids[soulPoint].GetPid(distanceToDestinationPoint.magnitude, Time.deltaTime);
            //add force
            force = Mathf.Clamp(force, 0, maxForcePoint);
            _soulPointsRbs[soulPoint].AddForce(directionToDestinationPoint * force);
            //limit speed
            _soulPointsRbs[soulPoint].velocity = Vector2.ClampMagnitude(_soulPointsRbs[soulPoint].velocity, maxSpeedPoint);
        }
    }

    private IEnumerator SpawnHeadAnimation()
    {
        //animation
        //Instantiate(spawnParticles, head.position, Quaternion.identity);
        head.position = soulController.transform.position;
        while (Vector2.Distance(head.transform.position, soulController.soulDestination) > errorThreshold)
        {
            var error = Vector2.Distance(head.transform.position, soulController.soulDestination);
            var direction = (soulController.soulDestination - head.transform.position).normalized;
            var force = spawnPid.GetPid(error, Time.deltaTime);
            force = Mathf.Clamp(force, 0, maxForce);
            rbHead.AddForce(direction * force);
            rbHead.velocity = Vector2.ClampMagnitude(rbHead.velocity, maxSpeed);
            yield return null;
        }
        
        //logic
        rbHead.velocity = Vector3.zero;
        soulState = SoulState.Moving;
        soulController.isActivated = true;
    }
    
    private IEnumerator DespawnHeadAnimation()
    {
        spawnPid.Reset();
        //animation
        while (Vector2.Distance(head.transform.position, player.transform.position) > errorThreshold)
        {
            var error = Vector2.Distance(head.transform.position, player.transform.position);
            var direction = (player.transform.position - head.transform.position).normalized;
            var force = spawnPid.GetPid(error, Time.deltaTime);
            force = Mathf.Clamp(force, 0, maxForce);
            rbHead.AddForce(direction * force);
            rbHead.velocity = Vector2.ClampMagnitude(rbHead.velocity, maxSpeed);
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
