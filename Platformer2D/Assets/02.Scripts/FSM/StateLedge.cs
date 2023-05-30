using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateLedge : State
{
    private enum Step
    {
        Start,
        Keep,
        End
    }
    private Step _step;
    private int _workflowIndex;
    private Vector2 _startPos;
    private Vector2 _endPos => _startPos + new Vector2(0.145f * movement.dir, 0.392f);
    private Rigidbody2D _rb;
    private LedgeDetector _ledgeDetector;

    public StateLedge(GameObject owner, int id, Func<bool> executionCondition, List<KeyValuePair<Func<bool>, int>> transitions, bool hasExitTime) : base(owner, id, executionCondition, transitions, hasExitTime)
    {
        _rb = owner.GetComponent<Rigidbody2D>();
        _ledgeDetector = owner.GetComponent<LedgeDetector>();
    }

    public override void Execute()
    {
        base.Execute();
        movement.StopMove();
        movement.isMovable = false;
        movement.isDirectionChangeable = false;
        _step = Step.Start;
        _workflowIndex = 0;
        owner.transform.position = _ledgeDetector.posDetected;
        _startPos = _ledgeDetector.posDetected;
        _rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public override void Stop()
    {
        base.Stop();
        _rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public override int Update()
    {
        switch (_step)
        {
            case Step.Start:
                return StartWorkflow();
            case Step.Keep:
                return KeepWorkflow();
            case Step.End:
                return EndWorkflow();
            default:
                return base.Update();
        }
    }



    private int StartWorkflow()
    {
        int nextID = id;

        switch (_workflowIndex)
        {
            case 0:
                {
                    animator.Play("LedgeStart");
                    _workflowIndex++;
                }
                break;
            case 1:
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        _step = Step.Keep;
                        _workflowIndex = 0;
                    }
                }
                break;
            default:
                break;
        }

        return nextID;
    }
    private int KeepWorkflow()
    {
        int nextID = id;

        switch (_workflowIndex)
        {
            case 0:
                {
                    animator.Play("LedgeKeep");
                    _workflowIndex++;
                }
                break;
            case 1:
                {
                    if (Input.GetKey(KeyCode.UpArrow))
                    {
                        _step = Step.End;
                        _workflowIndex = 0;
                    }
                }
                break;
            default:
                break;
        }

        return nextID;
    }
    private int EndWorkflow()
    {
        int nextID = id;

        switch (_workflowIndex)
        {
            case 0:
                {
                    animator.Play("LedgeEnd");
                    _workflowIndex++;
                }
                break;
            case 1:
                {
                    float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    if (normalizedTime >= 1.0f)
                    {
                        nextID = (int)StateMachine.StateType.Idle;
                    }
                    else
                    {
                        owner.transform.position = Vector2.Lerp(_startPos, _endPos, normalizedTime);
                    }
                }
                break;
            default:
                break;
        }

        return nextID;
    }
}
