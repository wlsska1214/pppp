using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack : State
{
    private GroundDetector _groundDetector;
    public StateAttack(GameObject owner, int id, Func<bool> executionCondition, List<KeyValuePair<Func<bool>, int>> transitions, bool hasExitTime) : base(owner, id, executionCondition, transitions, hasExitTime)
    {
        _groundDetector = owner.GetComponent<GroundDetector>();
    }

    public override void Execute()
    {
        base.Execute();
        if (_groundDetector.isDetected)
            movement.move = Vector2.zero;
        movement.isMovable = false;
        movement.isDirectionChangeable = false;
        animator.Play("Attack");
    }
}
