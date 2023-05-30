using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPUI : MonoBehaviour
{
    private Slider _hp;
    private float _duration = 3.0f;
    private float _deactiveTimer;

    private void Start()
    {
        _hp = transform.GetChild(0).GetComponent<Slider>();
        IDamageable subject = transform.parent.GetComponent<IDamageable>();
        _hp.minValue = subject.hpMin;
        _hp.maxValue = subject.hpMax;
        _hp.value = subject.hp;
        subject.OnHpDecreased += (value) =>
        {
            _hp.value = value;
            _deactiveTimer = _duration;
            gameObject.SetActive(true);
        };
        subject.OnHpIncreased += (value) =>
        {
            _hp.value = value;
            _deactiveTimer = _duration;
            gameObject.SetActive(true);
        };
    }

    private void Update()
    {
        if (_deactiveTimer > 0)
        {
            _deactiveTimer -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
