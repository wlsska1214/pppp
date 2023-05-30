using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{
    private TMP_Text _damage;
    private float _fadeSpeed = 1.0f;
    private float _moveSpeedY = 0.5f;
    private Color _color;

    public static DamagePopUp Create(LayerMask mask, Vector3 pos, int damage)
    {
        DamagePopUp damagePopUp = Instantiate(DamagePopUpAssets.instance[mask],
                                              pos,
                                              Quaternion.identity);
        damagePopUp._damage.SetText(damage.ToString());
        return damagePopUp;
    }

    private void Awake()
    {
        _damage = transform.GetChild(0).GetComponent<TMP_Text>();
        _color = _damage.color;
    }

    private void Update()
    {
        transform.position += Vector3.up * _moveSpeedY * Time.deltaTime;
        _color.a -= _fadeSpeed * Time.deltaTime;
        _damage.color = _color;

        if (_color.a <= 0.0f)
            Destroy(gameObject);
    }
}
