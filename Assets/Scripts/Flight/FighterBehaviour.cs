using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FighterState
{
    OFFCAMERA,
    NOENEMES,
    ENEMES,
}

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

    private FighterState myState;

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
        UpdateState();
        UpdateTarget(myState);
    }

    private void UpdateState()
    {
        if (!myRenderer.isVisible)
        {
            myState = FighterState.OFFCAMERA;
            return;
        }

        if (myRadar.AnyEnemys(myFaction))
        {
            myState = FighterState.ENEMES;
            return;
        }

        myState = FighterState.NOENEMES;
    }

    private void UpdateTarget(FighterState state)
    {
        switch (state)
        {
            case FighterState.OFFCAMERA:
                myTarget = cameraTransform.position;
                break;
            case FighterState.NOENEMES:
                myTarget = myRadar.ShipCentroid(myFaction);
                if (!myTarget.HasValue) myTarget = cameraTransform.position;
                break;
            case FighterState.ENEMES:
                myTarget = myRadar.GetClosestEnemy(myFaction);
                if (!myTarget.HasValue) myTarget = cameraTransform.position;
                break;
        }

        myController.SetTarget((Vector2)myTarget);
        
    }

}
