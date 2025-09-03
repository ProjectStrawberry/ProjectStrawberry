using UnityEngine;
using System;

public interface IPoolable
{
    void Initialize(Action<GameObject> returnAction);
    void OnSpawn();
    void OnDespawn();
}