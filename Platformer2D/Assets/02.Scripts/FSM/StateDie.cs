using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDie : State
{
    public StateDie(GameObject owner, int id, Func<bool> executionCondition, List<KeyValuePair<Func<bool>, int>> transitions, bool hasExitTime) : base(owner, id, executionCondition, transitions, hasExitTime)
    {
    }
    public override void Execute()
    {
        base.Execute();
        movement.StopMove();
        movement.isMovable = false;
        movement.isDirectionChangeable = false;
        animator.Play("Die");
    }
}
