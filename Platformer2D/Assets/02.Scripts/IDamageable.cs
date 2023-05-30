using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    int hp { get; set; }
    int hpMax { get; }
    int hpMin { get; }

    event Action<int> OnHpDecreased;
    event Action<int> OnHpIncreased;
    event Action OnHpMin;
    event Action OnHpMax;

    void Damage(GameObject hitter, int damage);
}
