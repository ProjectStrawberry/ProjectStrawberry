using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CrystalKnight", menuName = "Boss/CrystalKnight")]
public class CrystalKnightSO : ScriptableObject
{
    [field: SerializeField] public int health;
    [field: SerializeField] public int damage;
}
