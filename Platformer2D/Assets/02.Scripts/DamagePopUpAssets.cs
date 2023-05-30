using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUpAssets : MonoBehaviour
{
    private static DamagePopUpAssets _instance;
    public static DamagePopUpAssets instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Instantiate(Resources.Load<DamagePopUpAssets>("DamagePopUpAssets"));
                _instance.Init();
            }
            return _instance;
        }
    }

    public DamagePopUp this[LayerMask layerMask]
        => _dictionary[layerMask];

    [SerializeField] private List<UKeyValuePair<LayerMask, DamagePopUp>> _damagePopUps;
    private Dictionary<LayerMask, DamagePopUp> _dictionary;


    private void Init()
    {
        _dictionary = new Dictionary<LayerMask,DamagePopUp>();
        foreach (UKeyValuePair<LayerMask, DamagePopUp> damagePopUp in _damagePopUps)
        {
            _dictionary.Add(damagePopUp.key, damagePopUp.value);
        }
    }
}
