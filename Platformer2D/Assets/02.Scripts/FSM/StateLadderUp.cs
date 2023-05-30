using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateLadderUp : State
{
    private LadderDetector _ladderDetector;
    private GroundDetector _groundDetector;
    private Rigidbody2D _rb;
    public StateLadderUp(GameObject owner, int id, Func<bool> executionCondition, List<KeyValuePair<Func<bool>, int>> transitions, bool hasExitTime) : base(owner, id, executionCondition, transitions, hasExitTime)
    {
        _ladderDetector = owner.GetComponent<LadderDetector>();
        _groundDetector = owner.GetComponent<GroundDetector>();
        _rb = owner.GetComponent<Rigidbody2D>();
    }

    public override void Execute()
    {
        base.Execute();
        _rb.bodyType = RigidbodyType2D.Kinematic;
        movement.StopMove();
        movement.isMovable = false;
        movement.isDirectionChangeable = false;
        animator.Play("Ladder");
        animator.speed = 0.0f;
        owner.transform.position = _ladderDetector.GetClimbUpStartPos();
    }

    public override void Stop()
    {
        base.Stop();
        _rb.bodyType = RigidbodyType2D.Dynamic;
        animator.speed = 1.0f;
    }


    public override int Update()
    {
        owner.transform.position += Vector3.up * movement.v * Time.deltaTime;
        animator.speed = Mathf.Abs(movement.v);

        if (_ladderDetector.doEscapeUp)
        {
            owner.transform.position = _ladderDetector.latestUpLadderTopPos;
            return (int)StateMachine.StateType.Idle;
        }
        else if (_ladderDetector.doEscapeDown)
        {
            return (int)StateMachine.StateType.Idle;
        }
        else if (_groundDetector.isDetected)
        {
            return (int)StateMachine.StateType.Idle;
        }
        else if (Input.GetKey(KeyCode.LeftAlt))
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            movement.SetMove(new Vector2(movement.h,0));
            _rb.velocity = movement.move;
            machine.ChangeStateForcely((int)StateMachine.StateType.Jump);
            return (int)StateMachine.StateType.Jump;
        }

        return base.Update();
    }
}
