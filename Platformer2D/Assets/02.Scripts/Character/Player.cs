using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public StateMachine stateMachine;

    public int hp
    {
        get => _hp;
        set
        {
            if (_hp == value)
                return;

            int prev = _hp;
            _hp = value;

            if (value < prev)
            {
                DamagePopUp.Create(1 << gameObject.layer,
                                   transform.position + Vector3.up * 0.25f,
                                   prev - value);

                if (value <= hpMin)
                    OnHpMin?.Invoke();
                else
                    OnHpDecreased?.Invoke(value);
            }
            else
            {
                if (value >= hpMax)
                    OnHpMax?.Invoke();
                else
                    OnHpIncreased?.Invoke(value);
            }
        }
    }

    public int hpMax => _hpMax;

    public int hpMin => 0;

    public event Action<int> OnHpDecreased;
    public event Action<int> OnHpIncreased;
    public event Action OnHpMin;
    public event Action OnHpMax;
    [SerializeField] private int _damage = 20;
    private int _hp;
    [SerializeField] private int _hpMax = 100;
    public bool isInvincible;
    [SerializeField] private float _invincibleDuration = 0.5f;

    [SerializeField] private Vector2 _attackCastCenter;
    [SerializeField] private Vector2 _attackCastSize;
    [SerializeField] private LayerMask _targetMask;
    private Movement _movement;
    
    
    public void Damage(GameObject hitter, int damage)
    {
        if (isInvincible)
            return;

        hp -= damage;

        isInvincible = true;
        StartCoroutine(E_ReleaseInvincible());
    }

    IEnumerator E_ReleaseInvincible()
    {
        float timeMark = Time.time;
        while (Time.time - timeMark < _invincibleDuration)
        {
            yield return null;
        }
        isInvincible = false;
    }

    private void Awake()
    {
        hp = hpMax;
        stateMachine = new StateMachine(gameObject);
        OnHpDecreased += (value) => stateMachine.ChangeState((int)StateMachine.StateType.Hurt);
        OnHpMin += () => stateMachine.ChangeState((int)StateMachine.StateType.Die);
        _movement = GetComponent<Movement>();
    }

    private void Update()
    {
        bool changed = false;

        do
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                changed = stateMachine.ChangeState((int)StateMachine.StateType.Jump);
                if (changed) break;
                changed = stateMachine.ChangeState((int)StateMachine.StateType.DownJump);
                if (changed) break;
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                changed = stateMachine.ChangeState((int)StateMachine.StateType.LadderDown);
                if (changed) break;
                changed = stateMachine.ChangeState((int)StateMachine.StateType.Crouch);
                if (changed) break;
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                changed = stateMachine.ChangeState((int)StateMachine.StateType.LadderUp);
                if (changed) break;
                changed = stateMachine.ChangeState((int)StateMachine.StateType.Ledge);
                if (changed) break;
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                changed = stateMachine.ChangeState((int)StateMachine.StateType.Slide);
                if (changed) break;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                changed = stateMachine.ChangeState((int)StateMachine.StateType.Dash);
                if (changed) break;
            }

            if (Input.GetKey(KeyCode.A))
            {
                changed = stateMachine.ChangeState((int)StateMachine.StateType.Attack);
                if (changed) break;
            }
        } while (false);
        
        stateMachine.UpdateState();
    }

    private void Hit()
    {
        Collider2D target = Physics2D.OverlapBox((Vector2)transform.position + new Vector2(_attackCastCenter.x * _movement.dir, _attackCastCenter.y), _attackCastSize, 0.0f, _targetMask);
        if (target != null &&
            target.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(gameObject, _damage);
        }
    }

    private void OnDrawGizmos()
    {
        if (stateMachine != null &&
            stateMachine.currentStateID == (int)StateMachine.StateType.Attack)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + new Vector3(_attackCastCenter.x * _movement.dir, _attackCastCenter.y, 0.0f), _attackCastSize);
        }
    }
}
