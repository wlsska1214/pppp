using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Slider _hpBar;
    [SerializeField] private TMP_Text _hpValue;

    private void Start()
    {
        _hpBar.minValue = _player.hpMin;
        _hpBar.maxValue = _player.hpMax;
        _hpBar.value = _player.hp;
        _hpValue.text = _player.hp.ToString();

        _player.OnHpDecreased += (value) =>
        {
            _hpBar.value = value;
            _hpValue.text = value.ToString();
        };
        _player.OnHpIncreased += (value) =>
        {
            _hpBar.value = value;
            _hpValue.text = value.ToString();
        };
    }
}
