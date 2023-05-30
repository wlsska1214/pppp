using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWithDamage : Projectile
{
    protected GameObject owner;
    protected int damage;
    protected LayerMask targetMask;
    public void SetUp(GameObject owner, int direction, int damage, LayerMask targetMask)
    {
        this.owner = owner;
        this.direction = direction;
        this.damage = damage;
        this.targetMask = targetMask;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer & targetMask) > 0 &&
            collision.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.Damage(owner ,damage);
            Destroy(gameObject);
        }
    }
}
