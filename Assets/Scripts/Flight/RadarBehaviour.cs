using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarBehaviour : MonoBehaviour
{
    private List<GameObject> ships = new List<GameObject>();


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
