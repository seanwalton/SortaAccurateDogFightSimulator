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
    private IAttackable isAttackable;
    private Vector2 newVelocity;

    public Transform myTransform { private set; get; }

    private void Awake()
    {
        myTransform = transform;
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
        newVelocity = myType.speed * direction;
        newVelocity.x += Random.Range(-1f * (myType.speed * (1.0f - myType.accuracy)),
            (myType.speed * (1.0f - myType.accuracy)));
        newVelocity.y += Random.Range(-1f * (myType.speed * (1.0f - myType.accuracy)),
            (myType.speed * (1.0f - myType.accuracy)));

        rb.velocity = newVelocity;
        



        timeLeft = myType.lifetime;
    }

    private void Explode()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        isAttackable = collision.gameObject.GetComponentInParent<IAttackable>();
        if (isAttackable != null)
        {
            isAttackable.OnAttack(myType.damage);
            Explode();
        }
    }

}
