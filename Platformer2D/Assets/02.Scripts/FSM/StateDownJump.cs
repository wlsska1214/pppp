using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDownJump : State
{
    private GroundDetector _groundDetector;
    private Rigidbody2D _rb;
    private float _jumpForce = 1.0f;

    public StateDownJump(GameObject owner, int id, Func<bool> executionCondition, List<KeyValuePair<Func<bool>, int>> transitions, bool hasExitTime) 
        : base(owner, id, executionCondition, transitions, hasExitTime)
    {
        _groundDetector = owner.GetComponent<GroundDetector>();
        _rb = owner.GetComponent<Rigidbody2D>();
    }

    public override void Execute()
    {
        base.Execute();
        movement.isMovable = false;
        movement.isDirectionChangeable = true;
        animator.Play("Jump");

        _rb.velocity = new Vector2(_rb.velocity.x, 0.0f);
        _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        _groundDetector.IgnoreLatest();
    }

    public override int Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.1f)
            return id;

        return base.Update();
    }
}
