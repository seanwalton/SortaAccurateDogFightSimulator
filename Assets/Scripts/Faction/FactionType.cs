using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Faction
{
    RED,
    BLUE
}

public class FactionType : MonoBehaviour
{
    [SerializeField] private Faction faction;
    public Faction Faction => faction;
}
