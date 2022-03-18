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
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ShipController))]
public class FighterBehaviour : MonoBehaviour
{
    [SerializeField] private RadarBehaviour myRadar;
    private ShipController myController;
    private Vector2? myTarget;
    private ShipController myTargetSc;
    private Transform cameraTransform;
    private SpriteRenderer myRenderer;
    private Transform myTransform;
    private Faction myFaction;

    private FighterState myState;
    private GunBehaviour myGun;
    private Rigidbody2D myRb;
    private ShipController myShip;

    private void Awake()
    {
        myController = GetComponent<ShipController>();
        cameraTransform = Camera.main.transform;
        myRenderer = GetComponent<SpriteRenderer>();
        myTransform = transform;
        myFaction = GetComponent<FactionType>().Faction;
        myGun = GetComponentInChildren<GunBehaviour>();
        myRb = GetComponent<Rigidbody2D>();
        myShip = GetComponent<ShipController>();
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
            if (myState == FighterState.ENEMES)
            {
                myGun.StopFiring();
            }
            myState = FighterState.OFFCAMERA;
            return;
        }

        if (myRadar.AnyEnemys(myFaction))
        {
            if (myState != FighterState.ENEMES)
            {
                myGun.StartFiring(myFaction);
            }
            myState = FighterState.ENEMES;
            return;
        }

        if (myState == FighterState.ENEMES)
        {
            myGun.StopFiring();
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
                myTarget = myRadar.ShipCentroid(myFaction, myRb, 
                    myShip.maxSpeed);
                if (!myTarget.HasValue) myTarget = cameraTransform.position;
                break;
            case FighterState.ENEMES:
                myTargetSc = myRadar.GetClosestEnemyShipController(myFaction);
                if (myTargetSc)
                {
                    myTarget = myGun.LeadTarget(myTargetSc);
                }
                else
                {
                    myTarget = cameraTransform.position;
                }
                break;
        }

        myController.SetTarget((Vector2)myTarget);
        
    }

    

}
