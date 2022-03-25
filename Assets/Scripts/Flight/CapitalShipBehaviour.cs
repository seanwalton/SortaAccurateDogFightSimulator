using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipController))]
[RequireComponent(typeof(FactionType))]
public class CapitalShipBehaviour : MonoBehaviour
{
    private Vector2? myTarget;
    private Transform cameraTransform;
    private ShipController myController;


    private void Awake()
    {
        myController = GetComponent<ShipController>();
        cameraTransform = Camera.main.transform;
    }

    private void Start()
    {
        myController.SetTarget(cameraTransform.position);
    }
}
