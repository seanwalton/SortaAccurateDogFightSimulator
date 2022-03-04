using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthBehaviour : MonoBehaviour, IAttackable
{
    [SerializeField] private int startingHealth;
    [SerializeField] private UnityEvent OnKill;
    [SerializeField] private UnityEvent<float> OnTakeDamage;

    private int health;

    private void Awake()
    {
        health = startingHealth;
    }

    public void OnAttack(int damage)
    {
        health -= damage;
        OnTakeDamage?.Invoke(damage/(float)startingHealth);

        if (health <= 0) Kill();
    }

    private void Kill()
    {
        OnKill?.Invoke();
        gameObject.SetActive(false);
    }

}
