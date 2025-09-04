using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalKnightCondition : MonoBehaviour
{
    public CrystalKnight CrystalKnight;

    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    private void Awake()
    {
        CrystalKnight = GetComponent<CrystalKnight>();
        maxHealth = CrystalKnight.StatData.health;
        currentHealth = maxHealth;
    }
}
