using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] private ProjectileType myType;

    private Rigidbody2D rb;
    private float timeLeft;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();     
    }

    private void FixedUpdate()
    {
        if (timeLeft > 0f)
        {
            timeLeft -= Time.fixedDeltaTime;
            if (timeLeft <= 0f) Explode();
        }
    }

    public void OnFire(Vector2 direction)
    {
        rb.velocity = myType.speed * direction;
        timeLeft = myType.lifetime;
    }

    private void Explode()
    {
        gameObject.SetActive(false);
    }

}
