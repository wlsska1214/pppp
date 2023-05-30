using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCrouch : State
{
    private enum Step
    {
        Start,
        Keep,
        End
    }
    private Step _step;
    private int _workflowIndex;
    private Vector2 _colOffsetOrigin = new Vector2(0.0f, 0.17f);
    private Vector2 _colSizeOrigin = new Vector2(0.1f, 0.3f);
    private Vector2 _colOffsetCrouch = new Vector2(0.0f, 0.07f);
    private Vector2 _colSizeCrouch = new Vector2(0.1f, 0.1f);
    private CapsuleCollider2D[] _cols;

    public StateCrouch(GameObject owner, int id, Func<bool> executionCondition, List<KeyValuePair<Func<bool>, int>> transitions, bool hasExitTime) : base(owner, id, executionCondition, transitions, hasExitTime)
    {
        _cols = owner.GetComponentsInChildren<CapsuleCollider2D>();
    }

    public override void Execute()
    {
        base.Execute();
        movement.isMovable = false;
        movement.isDirectionChangeable = true;
        movement.StopMove();
        _step = Step.Start;
        _workflowIndex = 0;

        for (int i = 0; i < _cols.Length; i++)
        {
            _cols[i].size = _colSizeCrouch;
            _cols[i].offset = _colOffsetCrouch;
        }
    }

    public override void Stop()
    {
        base.Stop();
        for(int i = 0; i < _cols.Length; i++)
        {
            _cols[i].size = _colSizeOrigin;
            _cols[i].offset = _colOffsetOrigin;
        }
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
                    animator.Play("CrouchStart");
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

                    if (Input.GetKey(KeyCode.DownArrow) == false)
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

    private int KeepWorkflow()
    {
        int nextID = id;
        switch (_workflowIndex)
        {
            case 0:
                {
                    animator.Play("CrouchKeep");
                    _workflowIndex++;
                }
                break;
            case 1:
                {
                    if (Input.GetKey(KeyCode.DownArrow) == false)
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
                    animator.Play("CrouchEnd");
                    _workflowIndex++;
                }
                break;
            case 1:
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        nextID = (int)StateMachine.StateType.Idle;
                    }
                }
                break;
            default:
                break;
        }
        return nextID;
    }
}
