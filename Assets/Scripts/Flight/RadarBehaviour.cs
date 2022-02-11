using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarBehaviour : MonoBehaviour
{
    private List<GameObject> ships = new List<GameObject>();
    private Transform trTemp;
    private Vector2 centroid;

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
