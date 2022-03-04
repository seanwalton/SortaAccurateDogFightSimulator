using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ProjectileBehaviour : MonoBehaviour
{
    public ProjectileType myType;

    private Rigidbody2D rb;
    private float timeLeft;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().sprite = myType.mySprite;
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
        rb.velocity = Random.Range(myType.speed - 0.1f*myType.speed,
            myType.speed + 0.1f * myType.speed) * direction;
        timeLeft = myType.lifetime;
    }

    private void Explode()
    {
        gameObject.SetActive(false);
    }

}
