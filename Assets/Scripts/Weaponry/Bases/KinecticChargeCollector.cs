using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinecticChargeCollector : MonoBehaviour
{
    [SerializeField] protected TMPro.TextMeshProUGUI chargesIcon;
    protected int _charges;
    public int charges
    {
        get => _charges;
        set
        {
            _charges = value;
            chargesIcon.text = _charges.ToString();
        }
    }


    private void Start()
    {
        GetComponent<WeaponBase>().OnShoot.AddListener(() => charges = 0);
    }
}
