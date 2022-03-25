using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TurretState
{
    NOENEMES,
    ENEMES,
}

[RequireComponent(typeof(FactionType))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(ShipController))]
public class TurretController : MonoBehaviour
{
    [SerializeField] private RadarBehaviour myRadar;
    [SerializeField] private Transform myForward;
    private Vector2? myTarget;
    private GunBehaviour[] myGuns;
    private Faction myFaction;
    private TurretState myState;
    private Vector2 initialTarget;
    private ShipController myTargetSc;
    private ShipController myController;

    private void Awake()
    {
        myGuns = GetComponentsInChildren<GunBehaviour>();
        myFaction = GetComponent<FactionType>().Faction;
        myController = GetComponent<ShipController>();
    }

    private void Start()
    {
        initialTarget = transform.position + myForward.up;
    }

    private void FixedUpdate()
    {
        UpdateState();
        UpdateTarget(myState);
    }

    private void UpdateState()
    {
        if (myRadar.AnyEnemys(myFaction))
        {
            if (myState != TurretState.ENEMES)
            {
                for (int i = 0; i < myGuns.Length; i++)
                {
                    myGuns[i].StartFiring(myFaction);
                }
                
            }
            myState = TurretState.ENEMES;
            return;
        }

        if (myState == TurretState.ENEMES)
        {
            for (int i = 0; i < myGuns.Length; i++)
            {
                myGuns[i].StopFiring();
            }
        }
        myState = TurretState.NOENEMES;
    }

    private void UpdateTarget(TurretState state)
    {
        switch (state)
        {
            case TurretState.NOENEMES:
                myTarget = initialTarget;
                break;
            case TurretState.ENEMES:
                myTargetSc = myRadar.GetClosestEnemyShipController(myFaction);
                if (myTargetSc)
                {
                    myTarget = myGuns[0].LeadTarget(myTargetSc);
                }
                break;
        }

        myController.SetTarget((Vector2)myTarget);
    }

}
