using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMove : State
{
    public StateMove(GameObject owner, int id, Func<bool> executionCondition, List<KeyValuePair<Func<bool>, int>> transitions, bool hasExitTime)
        : base(owner, id, executionCondition, transitions, hasExitTime)
    {
    }

    public override void Execute()
    {
        base.Execute();
        movement.isMovable = true;
        movement.isDirectionChangeable = true;
        animator.Play("Move");
    }
}
