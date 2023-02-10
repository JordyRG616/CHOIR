using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using System;

public class WeaponBase : MonoBehaviour
{
    public int ID;
    public int cost;
    [TextArea][SerializeField] protected string description;
    [TextArea][SerializeField] public string Effect;
    [SerializeField] protected List<float> parameters;
    [TextArea] protected string processedDescription;
    [SerializeField] protected ParticleSystem MainShooter;
    [field:SerializeField] public List<ActionTile> tiles { get; protected set; }
    [Space]
    [SerializeField] public UnityEvent OnShoot, OnStop;
    public Vector2 damageRange;
    protected Animator anim;
    protected WeaponAudioController audioController;
    public WeaponGraphicsController graphicsController { get; protected set; }
    public bool unlocked = false;
    public WeaponClass classes;
    Regex rex = new Regex(@"&.\d");


    [field:SerializeField] public List<WeaponUpgrade> upgrades { get; private set; }

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        audioController = GetComponent<WeaponAudioController>();
        graphicsController = GetComponent<WeaponGraphicsController>();

        processedDescription = Description();

        upgrades.Add(new WeaponUpgrade(UpgradeTag.Estabilizer, ApplyEstabilizer));
        upgrades.Add(new WeaponUpgrade(UpgradeTag.Unstable, ApplyUnstable));
    }

    public virtual void ApplyPassiveEffect()
    {

    }

    private void ApplyUnstable()
    {
        damageRange.x = 0;
        damageRange.y *= 1.5f;
    }

    private void ApplyEstabilizer()
    {
        var average = (damageRange.x + damageRange.y) / 2;

        damageRange.x = average;
        damageRange.y = average;
    }

    public virtual void Shoot(WeaponKey key)
    {
        audioController.ChangeKey(key);
        OnShoot?.Invoke();
        anim.SetTrigger("Shoot");
    }

    public virtual void Stop()
    {
        OnStop?.Invoke();
    }

    public bool HasUpgradeTag(UpgradeTag tag, out WeaponUpgrade upgrade)
    {
        foreach (var _upgrade in upgrades)
        {
            if (_upgrade.tag == tag)
            {
                upgrade = _upgrade;
                return true;
            }
        }

        upgrade = null;
        return false;
    }

    public WeaponUpgrade FindUpgrade(UpgradeTag tag)
    {
        foreach (var upgrade in upgrades)
        {
            if (upgrade.tag == tag) return upgrade;
        }

        return null;
    }

    public string Description()
    {
        var matches = rex.Matches(description);
        var replacement = string.Empty;
        var _desc = new string(description);

        foreach(var match in matches)
        {
            var key = match.ToString().ToCharArray();
            var index = int.Parse(key[2].ToString());

            switch(key[1])
            {
                case 'd':
                    replacement = damageRange[index].ToString();
                    break;
                case 'p':
                    replacement = parameters[index].ToString();
                    break;
            }

            _desc = _desc.Replace(match.ToString(), replacement);
        }

        return _desc;
    }
}

[System.Flags]
public enum WeaponClass 
{ 
    Default = 0,
    Ballistic = 1,
    Laser = 2, 
    Flame = 4,
    Electric = 8
}

[System.Serializable]
public class WeaponUpgrade
{
    public UpgradeTag tag;
    public List<GameObject> objectsToActivate = new List<GameObject>();
    public List<GameObject> objectsToDeactivate = new List<GameObject>();
    public bool applied;

    public delegate void ApplyUpgrade();
    public ApplyUpgrade onUpgradedApplied;

    public WeaponUpgrade(UpgradeTag tag, ApplyUpgrade function)
    {
        this.tag = tag;
        onUpgradedApplied += function;
    }

    public void Apply()
    {
        onUpgradedApplied?.Invoke();
        
        objectsToActivate.ForEach(x => x.SetActive(true));
        objectsToDeactivate.ForEach(x => x.SetActive(false));

        applied = true;
    }
}
