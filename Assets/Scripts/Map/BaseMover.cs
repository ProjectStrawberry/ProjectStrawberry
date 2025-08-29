using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMover : MonoBehaviour
{
    private float movingSpeed = 1f;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }
    public void Move()
    {
        rb.MovePosition(rb.position + Vector2.down * movingSpeed * Time.fixedDeltaTime);
    }
}
