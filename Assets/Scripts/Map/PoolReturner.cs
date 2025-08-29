using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolReturner : MonoBehaviour
{
    [SerializeField] PlatformSpawner spawner;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Solid"))
        {
            spawner.ReturnToSolidPool(collision.gameObject);
        }
        else if(collision.CompareTag("SemiSolid"))
        {
            spawner.ReturnToSemiSolidPool(collision.gameObject);
        }
    }
}
