using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "ScriptableObjects/Projectile")]
public class ProjectileType : ScriptableObject
{
    public string projectileName;
    public int damage;
    public float speed;
    public float lifetime;
}
