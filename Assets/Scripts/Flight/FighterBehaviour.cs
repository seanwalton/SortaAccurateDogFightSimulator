using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipController))]
public class FighterBehaviour : MonoBehaviour
{
    private ShipController myController;
    

    private void Awake()
    {
        myController = GetComponent<ShipController>();
    }


}
