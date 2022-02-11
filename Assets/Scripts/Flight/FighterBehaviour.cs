using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipController))]
public class FighterBehaviour : MonoBehaviour
{

    [SerializeField] private RadarBehaviour myRadar;
    private ShipController myController;
    private Vector2? myTarget;

    private void Awake()
    {
        myController = GetComponent<ShipController>();
    }

    private void FixedUpdate()
    {
        UpdateTarget();
    }

    private void UpdateTarget()
    {
        myTarget = myRadar.ShipCentroid();

        if (myTarget.HasValue)
        {
            myController.SetTarget((Vector2) myTarget);
        }
        else
        {
            myController.SetTarget(new Vector2(0f,0f));
        }
    }

}
