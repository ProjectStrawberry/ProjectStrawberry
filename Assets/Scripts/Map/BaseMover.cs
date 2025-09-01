using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMover : MonoBehaviour
{
    public static float movingSpeed = 1f;
    Rigidbody2D rb;

    void Awake()
    {
        TryGetComponent(out rb); 
    }

    void Update()
    {
        if (rb == null)
        {
            transform.Translate(Vector2.down * movingSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.MovePosition(rb.position + Vector2.down * movingSpeed * Time.fixedDeltaTime);
        }
    }
}

