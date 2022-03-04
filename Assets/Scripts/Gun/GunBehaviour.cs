using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehaviour : MonoBehaviour
{
    [SerializeField] private float fireDelay;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int projectilePoolSize;

    private float nextFire;
    private bool isFiring;
    private List<ProjectileBehaviour> myPool = new List<ProjectileBehaviour>();
    private int currentBullet;
    private Transform tr;
    private float dist;
    private float timeToTarget;
    private float perlinX;
    private float perlinY;
    private float perlinWavelength;

    private void Awake()
    {
        InitalisePool();
        tr = transform;
    }

    private void InitalisePool()
    {
        for (int i = 0; i < projectilePoolSize; i++)
        {
            GameObject newProj = Instantiate(projectilePrefab);
            myPool.Add(newProj.GetComponent<ProjectileBehaviour>());
            newProj.SetActive(false);
        }
    }

    private void Start()
    {
        isFiring = false;
        currentBullet = 0;
        perlinX = UnityEngine.Random.value;
        perlinY = UnityEngine.Random.value;
        perlinWavelength = UnityEngine.Random.value;
    }

    private void FixedUpdate()
    {
        if (isFiring)
        {
            perlinX += perlinWavelength*Time.fixedDeltaTime;
            if (nextFire >= 0f)
            {
                nextFire -= Time.fixedDeltaTime;
                if (nextFire <= 0f) FireBullet();
            }
        }
    }

    private void FireBullet()
    {
        myPool[currentBullet].transform.position = tr.position;
        myPool[currentBullet].gameObject.SetActive(true);
        myPool[currentBullet].OnFire(tr.up);
        nextFire = fireDelay;
        currentBullet++;
        if (currentBullet > projectilePoolSize - 1) currentBullet = 0;
    }

    public void StartFiring()
    {
        if (!isFiring) nextFire = 0f;
        isFiring = true;
        
    }

    public void StopFiring()
    {
        isFiring = false;
    }

    public Vector2 LeadTarget(Rigidbody2D targetRb)
    {
        dist = Vector2.Distance(tr.position, targetRb.transform.position);
        timeToTarget = dist /
            ((1f + 0.5f*((Mathf.PerlinNoise(perlinX, perlinY) - 0.5f)*2f))*
            myPool[currentBullet].myType.speed);

        Vector2 lead = new Vector2();           
        lead.x = targetRb.transform.position.x + targetRb.velocity.x * timeToTarget;
        lead.y = targetRb.transform.position.y + targetRb.velocity.y * timeToTarget;
        return lead;
    }


}
