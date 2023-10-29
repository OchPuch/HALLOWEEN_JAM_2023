using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IKillable
{
    public AudioSource audioSource;
    public AudioClip deathSound;
    public AudioClip shootSound;
    public AudioClip reloadSound;
    public AudioClip blinkSound;
    
    public void PlayDeathSound()
    {
        audioSource.clip = deathSound;
        audioSource.Play();
    }
    
    public void PlayShootSound()
    {
        audioSource.clip = shootSound;
        audioSource.Play();
    }
    
    public void PlayReloadSound()
    {
        audioSource.clip = reloadSound;
        audioSource.Play();
    }
    
    public void PlayBlinkSound()
    {
        audioSource.clip = blinkSound;
        audioSource.Play();
    }
    
        
    public GameObject head;
    public GameObject gun;

    public TrackType trackType;

    public enum TrackType
    {
        AlwaysTrackPlayerWithGun,
        TrackPlayerOnSight,
        DontTrackPlayer
    }

    public bool raycastShoot;

    public bool trackPlayerWithHead = false;

    public bool isShooting = false;
    public bool isAiming = false;
    public bool isBlinking;

    public int maxAmmo;
    public int ammo;

    public float reloadTime;
    public float shootDelay;
    public float aimTime;
    public float blinkTime;

    public Transform player;
    public Transform bulletSpawnPoint;
    public Transform laserVisualStartPoint;

    public GameObject bulletPrefab;

    public GameObject laserVisual; //Показывает куда смотрит враг
    public SpriteRenderer laserSr;
    public Color aimingColor;
    public Color shootingColor;
    public Color waitingColor;
    public Color blinkColor;

    public Quaternion startRotation;

    private void Start()
    {
        ammo = maxAmmo;
        player = PlayerController.Instance.transform;
        laserVisual.transform.SetParent(null);
        startRotation = gun.transform.rotation;
    }

    void Update()
    {
        if (trackPlayerWithHead) TrackPlayerWithHead();
        if (trackType == TrackType.AlwaysTrackPlayerWithGun) TrackPlayerWithGun();
        UpdateLaserVisual();
    }

    void TrackPlayerWithHead()
    {
        Vector3 dir = player.position - head.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        head.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void TrackPlayerWithGun()
    {
        Vector3 dir = player.position - gun.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        gun.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


    void UpdateLaserVisual()
    {
        if (isBlinking) laserSr.color = blinkColor;
        else if (isAiming) laserSr.color = aimingColor;
        else if (isShooting) laserSr.color = shootingColor;
        else laserSr.color = waitingColor;


        RaycastHit2D hit = Physics2D.Raycast(bulletSpawnPoint.position, gun.transform.right);
        if (!hit.collider) return;
        if (hit.collider.gameObject.CompareTag("Player"))
        {
            if (!isShooting) StartCoroutine(StartShooting());
            if (trackType == TrackType.TrackPlayerOnSight) TrackPlayerWithGun();
        }
        else if (trackType == TrackType.TrackPlayerOnSight)
        {
            gun.transform.rotation = startRotation;
        }

        Vector2 midPoint = ((Vector2)laserVisualStartPoint.position + hit.point) / 2;
        laserVisual.transform.position = midPoint;
        laserVisual.transform.localScale = new Vector3(Vector2.Distance(laserVisualStartPoint.position, hit.point),
            laserVisual.transform.localScale.y, laserVisual.transform.localScale.z);
        Vector2 dir = hit.point - (Vector2)laserVisualStartPoint.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        laserVisual.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private IEnumerator StartShooting()
    {
        isShooting = true;
        isAiming = true;
        yield return new WaitForSeconds(aimTime);
        isAiming = false;
        //Raycast from gun if player still there then shoot
        RaycastHit2D hit = Physics2D.Raycast(bulletSpawnPoint.position, gun.transform.right);
        if (!hit.collider)
        {
            isShooting = false;
            yield break;
        }

        if (hit.collider.gameObject.CompareTag("Player"))
        {
            isBlinking = true;
            PlayBlinkSound();
            yield return new WaitForSeconds(blinkTime);
            
            //Check if player still there
            hit = Physics2D.Raycast(bulletSpawnPoint.position, gun.transform.right);
            if (!hit.collider.CompareTag("Player"))
            {
                isShooting = false;
                isBlinking = false;
                yield break;
            }
            
            isBlinking = false;

            while (true)
            {
                Shoot();
                if (ammo <= 0)
                {
                    PlayReloadSound();
                    yield return new WaitForSeconds(reloadTime);
                    ammo = maxAmmo;
                    isShooting = false;
                    yield break;
                }

                yield return new WaitForSeconds(shootDelay);
            }
        }


        isShooting = false;
    }

    public void Shoot()
    {
        PlayShootSound();
        if (raycastShoot)
        {
            PlayerController.Instance.Kill();
        }
        else
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.position, gun.transform.rotation);
        }

        ammo--;
    }


    public void Kill()
    {
        PlayDeathSound();
        Destroy(gameObject);
        Destroy(laserVisual);
    }
}