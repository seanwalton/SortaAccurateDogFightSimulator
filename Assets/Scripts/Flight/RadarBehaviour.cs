using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarBehaviour : MonoBehaviour
{
    private List<ShipController> ships = new List<ShipController>();
    private FactionType factionTemp;
    private Transform trTemp;
    private int numShips;
    private Vector2 centroid;
    private Vector2 direction;
    private Transform tr;
    private float dist;
    private float timeToTarget;
    private int closestI;
    private float distI;
    private float closestShipDist;

    private void Awake()
    {
        tr = transform;
    }

    public bool AnyEnemys(Faction myFaction)
    {
        for (int i = 0; i < ships.Count; i++)
        {
            factionTemp = ships[i].myFaction;
            if (factionTemp.Faction != myFaction)
            {
                return true;
            }
        }

        return false;
    }

    public Vector2? GetClosestEnemy(Faction myFaction)
    {             
        return GetClosestEnemyGameObject(myFaction).myTransform.position;
    }

    private ShipController GetClosestEnemyGameObject(Faction myFaction)
    {
        dist = float.MaxValue;
        closestI = -1;

        for (int i = 0; i < ships.Count; i++)
        {
            factionTemp = ships[i].myFaction;
            if (factionTemp.Faction != myFaction)
            {
                distI = Vector2.Distance(ships[i].myTransform.position, tr.position);
                if (distI < dist)
                {
                    dist = distI;
                    closestI = i;
                }
            }
        }
        if (closestI == -1) return null;
        return ships[closestI];
    }

    public Rigidbody2D GetClosestEnemyRb(Faction myFaction)
    {
        ShipController closest = GetClosestEnemyGameObject(myFaction);
        if (closest != null)
        {
            return closest.rb2D;
        }
        else
        {
            return null;
        }
    }

    public Vector2? ShipCentroid(Faction faction, Rigidbody2D myRb, 
        float mySpeed)
    {
        numShips = 0;

        centroid.Set(0f, 0f);
        direction.Set(0f, 0f);

        closestShipDist = float.MaxValue;

        for (int i = 0; i < ships.Count; i++)
        {
            factionTemp = ships[i].myFaction;
            if (factionTemp)
            {
                if (factionTemp.Faction == faction)
                {
                    trTemp = ships[i].myTransform;
                    centroid.x += trTemp.position.x;
                    centroid.y += trTemp.position.y;
                    numShips++;

                    dist = Vector2.Distance(tr.position,
                        ships[i].myTransform.position);

                    if (dist < closestShipDist)
                    {
                        direction.x = ships[i].myTransform.up.x;
                        direction.y = ships[i].myTransform.up.y;
                        closestShipDist = dist;
                    }

                    
                    
                }
            }

        }

        if (numShips == 0) return null;

        centroid.x /= numShips;
        centroid.y /= numShips;

        direction.x /= numShips;
        direction.y /= numShips;
        direction.Normalize();

        dist = Vector2.Distance(tr.position, centroid);
        timeToTarget = dist / mySpeed;

        centroid.x += direction.x * timeToTarget;
        centroid.y += direction.y * timeToTarget;

        return centroid;
    }


    public Vector2? ShipCentroid()
    {
        if (ships.Count == 0) return null;

        centroid = new Vector2(0f, 0f);
        for (int i = 0; i < ships.Count; i++)
        {
            trTemp = ships[i].myTransform;
            centroid.x += trTemp.position.x;
            centroid.y += trTemp.position.y;
        }

        centroid.x /= ships.Count;
        centroid.y /= ships.Count;

        return centroid;
    }

    public Vector2? ShipCentroid(Faction faction)
    {
        
        numShips = 0;

        centroid = new Vector2(0f, 0f);
        for (int i = 0; i < ships.Count; i++)
        {
            factionTemp = ships[i].myFaction;
            if (factionTemp)
            {
                if (factionTemp.Faction == faction)
                {
                    trTemp = ships[i].myTransform;
                    centroid.x += trTemp.position.x;
                    centroid.y += trTemp.position.y;
                    numShips++;
                }
            }
            
        }

        if (numShips == 0) return null;

        centroid.x /= numShips;
        centroid.y /= numShips;

        return centroid;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        ShipController newShip = collision.gameObject.GetComponent<ShipController>();

        if (newShip)
        {
            if (!ships.Contains(newShip))
            {
                ships.Add(newShip);
            }
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ShipController oldShip = collision.gameObject.GetComponent<ShipController>();

        if (ships.Contains(oldShip))
        {
            ships.Remove(oldShip);
        }
    }
}
