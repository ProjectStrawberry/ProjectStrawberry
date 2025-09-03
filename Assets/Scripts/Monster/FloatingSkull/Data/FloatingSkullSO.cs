using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FloatingSkull", menuName = "Enemy/FloatingSkull")]
public class FloatingSkullSO : ScriptableObject
{
    [field: SerializeField] public int damage = 1;
    [field: SerializeField] public int health = 2;
    [field: SerializeField] public float playerDetectRange  = 10f;
    [field: SerializeField] public float projectileFireRate  = 2.5f;
}
