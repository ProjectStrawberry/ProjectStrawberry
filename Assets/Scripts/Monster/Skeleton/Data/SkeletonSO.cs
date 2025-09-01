using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skeleton", menuName = "Enemy/Skeleton")]
public class SkeletonSO : ScriptableObject
{
    [field: SerializeField] public float PlayerChasingRange { get; private set; } = 5f;
    [field: SerializeField] public float DefaultAttackRange { get; private set; } = 2f;
    [field: SerializeField] public float RushAttackRange { get; private set; } = 2f;
    [field: SerializeField] public int damage = 1;
    [field: SerializeField] public int health = 3;
    [field: SerializeField] public float walkSpeed = 2f;
}
