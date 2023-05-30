using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NepenthesController : EnemyController
{
    [SerializeField] private Vector2 _attackBoxCenter;
    [SerializeField] private Vector2 _attackBoxSize;
    protected override void Hit()
    {
        base.Hit();
        Collider2D target = Physics2D.OverlapBox((Vector2)transform.position + new Vector2(_attackBoxCenter.x * direction, _attackBoxCenter.y), _attackBoxSize, 0.0f, aiTargetMask);
        if (target != null &&
            target.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(gameObject, (int)(damage * 1.5f));
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position + new Vector3(_attackBoxCenter.x * direction, _attackBoxCenter.y, 0.0f), _attackBoxSize);
    }
}
