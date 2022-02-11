using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipController))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(FactionType))]
public class FighterBehaviour : MonoBehaviour
{

    [SerializeField] private RadarBehaviour myRadar;
    private ShipController myController;
    private Vector2? myTarget;
    private Transform cameraTransform;
    private SpriteRenderer myRenderer;
    private Transform myTransform;
    private Faction myFaction;

    private void Awake()
    {
        myController = GetComponent<ShipController>();
        cameraTransform = Camera.main.transform;
        myRenderer = GetComponent<SpriteRenderer>();
        myTransform = transform;
        myFaction = GetComponent<FactionType>().Faction;
    }

    private void FixedUpdate()
    {
        UpdateTarget();
    }

    private void UpdateTarget()
    {
        myTarget = myRadar.ShipCentroid(myFaction);

        if (myTarget.HasValue)
        {
            if (myRenderer.isVisible)
            {
                myController.SetTarget((Vector2) myTarget);
            }
            else
            {
                myController.SetTarget(cameraTransform.position);
            }
            
        }
        else
        {
            if (myRenderer.isVisible)
            {
                myController.SetTarget(cameraTransform.position);
            }
            else
            {
                myController.SetTarget(myTransform.position);
            }
            
        }
    }

}
