using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : IState
{
    public int id { get; set; }
    public StateMachine machine { get; set; }
    public bool canExecute => _canExecute.Invoke();

    public List<KeyValuePair<Func<bool>, int>> transitions { get; set; }

    private Func<bool> _canExecute;
    protected bool hasExitTime;
    protected GameObject owner;
    protected Animator animator;
    protected Movement movement;

    public State(GameObject owner, int id, Func<bool> executionCondition, List<KeyValuePair<Func<bool>, int>> transitions, bool hasExitTime)
    {
        this.owner = owner;
        this.id = id;
        _canExecute = executionCondition;
        this.transitions = transitions;

        this.hasExitTime = hasExitTime;
        animator = owner.GetComponent<Animator>();
        movement = owner.GetComponent<Movement>();
    }


    public virtual void Execute()
    {
    }

    public virtual void Stop()
    {
    }

    /// <summary>
    /// 현재 상태의 로직을 수행하기위한 함수
    /// </summary>
    /// <returns> 전환하려는 다음 상태 ID </returns>
    public virtual int Update()
    {
        int nextID = id;

        if (hasExitTime == false ||
            (hasExitTime && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f))
        {

            foreach (KeyValuePair<Func<bool>, int> transition in transitions)
            {
                if (transition.Key.Invoke())
                {
                    nextID = transition.Value;
                    break;
                }
            }
        }

        return nextID;
    }
}