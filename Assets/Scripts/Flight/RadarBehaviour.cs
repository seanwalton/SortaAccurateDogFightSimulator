using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarBehaviour : MonoBehaviour
{
    private List<GameObject> ships = new List<GameObject>();
    private FactionType factionTemp;
    private Transform trTemp;
    private int numShips;
    private Vector2 centroid;
    private Transform tr;
    private float dist;
    private int closestI;
    private float distI;

    private void Awake()
    {
        tr = transform;
    }

    public bool AnyEnemys(Faction myFaction)
    {
        for (int i = 0; i < ships.Count; i++)
        {
            factionTemp = ships[i].GetComponent<FactionType>();
            if (factionTemp.Faction != myFaction)
            {
                return true;
            }
        }

        return false;
    }

    public Vector2? GetClosestEnemy(Faction myFaction)
    {
        dist = float.MaxValue;
        closestI = -1;

        for (int i = 0; i < ships.Count; i++)
        {
            factionTemp = ships[i].GetComponent<FactionType>();
            if (factionTemp.Faction != myFaction)
            {
                distI = Vector2.Distance(ships[i].transform.position, tr.position);
                if (distI < dist)
                {
                    dist = distI;
                    closestI = i;
                }
            }
        }

        if (closestI == -1) return null;
        return ships[closestI].transform.position;
    }

    public Vector2? ShipCentroid()
    {
        if (ships.Count == 0) return null;

        centroid = new Vector2(0f, 0f);
        for (int i = 0; i < ships.Count; i++)
        {
            trTemp = ships[i].transform;
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
            factionTemp = ships[i].GetComponent<FactionType>();
            if (factionTemp)
            {
                if (factionTemp.Faction == faction)
                {
                    trTemp = ships[i].transform;
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
        if (collision.gameObject.GetComponent<ShipController>())
        {
            if (!ships.Contains(collision.gameObject))
            {
                ships.Add(collision.gameObject);
            }
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (ships.Contains(collision.gameObject))
        {
            ships.Remove(collision.gameObject);
        }
    }
}
