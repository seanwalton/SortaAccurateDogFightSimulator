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
    private Vector2 lead;
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
        myPool[currentBullet].myTransform.position = tr.position;
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

    public Vector2 LeadTarget(ShipController target)
    {
        dist = Vector2.Distance(tr.position, target.myTransform.position);
        timeToTarget = dist /
            ((1f + 0.5f*((Mathf.PerlinNoise(perlinX, perlinY) - 0.5f)*2f))*
            myPool[currentBullet].myType.speed);

        lead.Set(0f, 0f);          
        lead.x = target.myTransform.position.x + target.rb2D.velocity.x * timeToTarget;
        lead.y = target.myTransform.position.y + target.rb2D.velocity.y * timeToTarget;
        return lead;
    }


}
