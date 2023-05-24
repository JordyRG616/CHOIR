using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : WeaponBase
{
    [SerializeField] private ParticleSystem coating;
    [SerializeField] private Color electricColor;
    private Color ogColor;
    [SerializeField] private GameObject sparks;


    private void Start()
    {
        ogColor = coating.main.startColor.color;        
    }

    public override void LevelUpEffect()
    {
        level++;

        switch(level)
        {
            case 2:
                damageRange.y += 2;
            break;
            case 3:
                damageRange += Vector2.one * 3;
            break;
            case 4:
                damageRange.x += 3;
            break;
            case 5:
                var main = MainShooter.main;
                main.startLifetime = new ParticleSystem.MinMaxCurve(1.25f);
            break;
        }
    }

    public override string WeaponDescription()
    {
        return "While active, shoots a continuos beam of energy that hits one enemy, dealing " + damageRange.x + " - " + damageRange.y + " continuous damage and applying <color=purple>frailty</color>.";
    }

    protected override void ApplyPerk()
    {
        var main = coating.main;
        main.startColor = new ParticleSystem.MinMaxGradient(electricColor);
        sparks.SetActive(true);
    }

    protected override void RemovePerk()
    {
        var main = coating.main;
        main.startColor = new ParticleSystem.MinMaxGradient(ogColor);
        sparks.SetActive(false);
    }
}
