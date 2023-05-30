using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkNepenthesController : EnemyController
{
    [SerializeField] private ProjectileWithDamage _projectilePrefab;

    protected override void Hit()
    {
        base.Hit();
        Instantiate(_projectilePrefab, transform.position + Vector3.up * 0.16f, Quaternion.identity)
            .SetUp(gameObject, direction, damage, aiTargetMask);
    }
}
