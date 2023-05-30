using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float speed = 2.0f;
    protected int direction; // 1: right, -1: left

    public void SetUp(int direction)
    {
        this.direction = direction;
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected virtual void Move()
    {
        transform.position += Vector3.right * direction * Time.fixedDeltaTime;
    }
}
