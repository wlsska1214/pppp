using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSlide : State
{
    private Vector2 _colOffsetOrigin = new Vector2(0.0f, 0.17f);
    private Vector2 _colSizeOrigin = new Vector2(0.1f, 0.3f);
    private Vector2 _colOffsetCrouch = new Vector2(0.0f, 0.07f);
    private Vector2 _colSizeCrouch = new Vector2(0.1f, 0.1f);
    private CapsuleCollider2D[] _cols;

    public StateSlide(GameObject owner, int id, Func<bool> executionCondition, List<KeyValuePair<Func<bool>, int>> transitions, bool hasExitTime) : base(owner, id, executionCondition, transitions, hasExitTime)
    {
        _cols = owner.GetComponentsInChildren<CapsuleCollider2D>();
    }

    public override void Execute()
    {
        base.Execute();
        movement.isMovable = false;
        movement.isDirectionChangeable = false;

        for (int i = 0; i < _cols.Length; i++)
        {
            _cols[i].size = _colSizeCrouch;
            _cols[i].offset = _colOffsetCrouch;
        }

        animator.Play("Slide");
    }

    public override void Stop()
    {
        base.Stop();

        for (int i = 0; i < _cols.Length; i++)
        {
            _cols[i].size = _colSizeOrigin;
            _cols[i].offset = _colOffsetOrigin;
        }
    }
}
