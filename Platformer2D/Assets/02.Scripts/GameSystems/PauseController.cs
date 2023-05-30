using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static PauseController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Instantiate(Resources.Load<PauseController>("PauseController"));
            }
            return _instance;
        }
    }
    private static PauseController _instance;

    public enum State
    {
        None,
        Playing,
        Paused
    }
    public State state;

    private List<IPauseable> _subscribers = new List<IPauseable>();

    public void Register(IPauseable subscriber)
    {
        _subscribers.Add(subscriber);
        subscriber.Pause(state == State.Paused);
    }

    public void Pause(bool pause)
    {
        foreach (var subscriber in _subscribers)
        {
            subscriber.Pause(pause);
        }
        state = pause ? State.Paused : State.Playing;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            state = state == State.Paused ? State.Playing : State.Paused;
            Pause(state == State.Paused);
        }
    }
}
