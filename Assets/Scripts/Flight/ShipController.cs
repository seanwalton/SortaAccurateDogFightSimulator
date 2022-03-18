using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : MonoBehaviour
{
    [SerializeField] private Transform myForward;
    [SerializeField] private float mySpeed;
    [SerializeField] private float maxRotationSpeed;
    
    private Vector2 myTarget = new Vector2(0f, 0f);
    public Rigidbody2D rb2D { private set; get; }
    public FactionType myFaction { private set; get; }
    private float currentRotationSpeed = 0f;
    private float signedAngle;
    private Vector2 toTarget = new Vector2();
    public Transform myTransform { private set; get; }

    public float maxSpeed => mySpeed;

    public void SetTarget(Vector2 newTarget)
    {
        myTarget = newTarget;
    }

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        myFaction = GetComponent<FactionType>();
        myTransform = transform;
    }


    private void FixedUpdate()
    {
        UpdateRotation();
        UpdateVelocity();      
    }

    private void UpdateVelocity()
    {
        rb2D.velocity = mySpeed * myForward.up;
    }

    private void UpdateRotation()
    {
        toTarget.x = myTarget.x - myTransform.position.x;
        toTarget.y = myTarget.y - myTransform.position.y;

        signedAngle = Vector2.SignedAngle(myForward.up, toTarget);

        currentRotationSpeed = signedAngle / 180f;

        rb2D.angularVelocity = currentRotationSpeed * maxRotationSpeed;

    }

    
}
