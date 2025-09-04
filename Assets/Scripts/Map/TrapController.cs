using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum TrapType
{
    Spikes,
    Razor
}

public class TrapController : MonoBehaviour
{
    [SerializeField] TrapType trapType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(trapType == TrapType.Spikes)
        {
            collision.gameObject.GetComponent<PlayerCondition>()?.GetDamage(1);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(trapType==TrapType.Razor)
        {
            collision.gameObject.GetComponent<PlayerCondition>()?.GetDamage(1);
        }
    }

    
}
