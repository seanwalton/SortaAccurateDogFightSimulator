using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehaviour : MonoBehaviour
{
    [SerializeField] private float fireDelay;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int projectilePoolSize;
    [SerializeField] private LayerMask clearShotLayer;

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
    private Faction faction;
    private RaycastHit2D hit;
    private FactionType otherFaction;
    private float circleCastRadius;

    private void Awake()
    {
        InitalisePool();
        tr = transform;
        CalcCircleRadius();
    }

    private void CalcCircleRadius()
    {
        SpriteRenderer spriteR = GetComponentInParent<SpriteRenderer>();

        circleCastRadius = Mathf.Max(spriteR.bounds.size.x*0.5f, spriteR.bounds.size.y*0.5f);
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
        if (!ClearShot())
        {          
            return;
        }

        myPool[currentBullet].myTransform.position = tr.position;
        myPool[currentBullet].gameObject.SetActive(true);
        myPool[currentBullet].OnFire(tr.up);
        nextFire = fireDelay;
        currentBullet++;
        if (currentBullet > projectilePoolSize - 1) currentBullet = 0;
    }

    public bool ClearShot()
    {
        hit = Physics2D.CircleCast(tr.position + (circleCastRadius*tr.up), 
            circleCastRadius, tr.up, Mathf.Infinity, clearShotLayer);

        if (hit.collider != null)
        {
            otherFaction = hit.collider.GetComponentInParent<FactionType>();
            if (otherFaction)
            {
                return (otherFaction.Faction != faction);
            }
            else
            {
                return true;
            }
        }
        else
        {
            return true;
        }
    }

    public void StartFiring(Faction myFaction)
    {
        faction = myFaction;
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
