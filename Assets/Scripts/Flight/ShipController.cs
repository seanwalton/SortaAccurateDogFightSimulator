using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipController : MonoBehaviour
{
    [SerializeField] private Transform myForward;
    [SerializeField] private float mySpeed;
    [SerializeField] private float myRotationSpeed;

    private Rigidbody2D rb2D;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        UpdateVelocity();
        UpdateRotation();
    }

    private void UpdateVelocity()
    {
        rb2D.velocity = mySpeed * myForward.up;
    }

    private void UpdateRotation()
    {
        rb2D.angularVelocity = myRotationSpeed;
    }

}
