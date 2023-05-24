using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using UnityEngine.Rendering.Universal;

public class CrystalManager : MonoBehaviour
{
    #region Main
    private static CrystalManager _instance;
    public static CrystalManager Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<CrystalManager>();

            return _instance;
        }

    }
    #endregion

    private Animator anim;
    [SerializeField] private CameraManager cameraManager;

    [Header("Health")]
    [SerializeField] private int maxHealth;
    [SerializeField] private TextMeshProUGUI currentValue;
    public int currentHealth { get; private set; }

    [Header("Experience Information")]
    [SerializeField] private int requiredExperience;
    [SerializeField] private AnimationCurve increment;
    //[SerializeField] private ParticleSystem gainExpVFX;
    [SerializeField] private StudioEventEmitter gainExpSFX;
    public int level { get; private set; } = 0;
    private int currentExperience;
    private int experienceHolder;
    [field: SerializeField] public int buildPoints { get; private set; }

    [Header("Modules")]
    [SerializeField] private Transform boomboxPoint;
    [SerializeField] private Vector2 boomboxSize;
    [SerializeField] private float boomboxForce;
    
    [Space]
    [SerializeField] private int healAmount;

    [Header("VFX")]
    [SerializeField] private ParticleSystem waveformBurst;
    [SerializeField] private ParticleSystem waveformDamage;
    [SerializeField] private ParticleSystem cashUp;
    [SerializeField] private Material fillMat;

    [Header("Cash UI")]
    [SerializeField] private TextMeshProUGUI points;

    

    public delegate void LevelUpEvent(int level);
    public LevelUpEvent onLevelUp;
    private bool blinkingCash;
    private bool blinkingHealth = false;
    private bool waitingLevelUp;

    private void Start()
    {
        anim = GetComponent<Animator>();
        maxHealth = Inventory.Main.currentMaxHealth;
        currentHealth = maxHealth;
        currentExperience = 0;
        SetHealthValue();
        points.text = buildPoints.ToString() + " $";

        fillMat.SetFloat("_Fill", 0);
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "EXP") GainExp();
        if(other.tag == "Heal")
        {
            currentHealth += healAmount;
            SetHealthValue();
        }
    }

    private void GainExp()
    {
        if (waitingLevelUp) experienceHolder += 1;
        else currentExperience += 1;
        gainExpSFX.Play();

        if (currentExperience >= requiredExperience)
        {
            EnqueueLevelUp();
            currentExperience = 0;
        }

        //gainExpVFX.Play();
    }

    public void DoWaveformBurst()
    {
        waveformBurst.Play();
    }

    public void BlinkCost()
    {
        if (blinkingCash) return;
        StartCoroutine(BlinkCash());
    }

    private IEnumerator BlinkCash()
    {
        blinkingCash = true;
        var ogTextColor = points.color;

        points.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        points.color = ogTextColor;
        blinkingCash = false;
    }

    public void ExpendBuildPoints(int value)
    {
        buildPoints -= value;

        points.text = buildPoints.ToString() + " $";
    }

    private void EnqueueLevelUp()
    {
        if(SpawnerManager.Main.OnWave)
        {
            waitingLevelUp = true;
            ActionMarker.Main.OnBeat += LevelUp;
        } else LevelUp();
    }

    private void LevelUp()
    {
        ActionMarker.Main.OnBeat -= LevelUp;

        requiredExperience = Mathf.RoundToInt(increment.Evaluate(level));
        level++;
        buildPoints++;
        points.text = buildPoints.ToString() + " $";
        cashUp.Play();
        AudioManager.Main.RequestEvent(FixedAudioEvent.CashUp);

        onLevelUp?.Invoke(level);

        waitingLevelUp = false;
        currentExperience += experienceHolder;
        experienceHolder = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<EnemyDamageModule>(out var damageController))
        {
            TakeDamage(collision, damageController);
        }
    }

    private void TakeDamage(Collider2D collision, EnemyDamageModule damageController)
    {
        currentHealth -= damageController.damage;
        EndGameLog.Main.damageTaken += damageController.damage;

        cameraManager.DoShake();
        waveformDamage.Play();
        AudioManager.Main.RequestEvent(FixedAudioEvent.TakeDamage);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            EndGameLog.Main.TriggerEndgame(false);
        }

        SetHealthValue();
        if (!blinkingHealth) StartCoroutine(BlinkDamage());
        collision.GetComponent<EnemyHealthModule>().DieFromAttacking();
        if(ModuleActivationManager.Main.HasSpecialEffect(ModuleSpecialEffect.BoomBox)) DoBoomBox();
    }

    private void DoBoomBox()
    {
        var hits = Physics2D.BoxCastAll(boomboxPoint.position, boomboxSize, 0, transform.forward, 0, LayerMask.GetMask("Enemies"));
        
        foreach(var hit in hits)
        {
            var march = hit.collider.GetComponent<EnemyMarchModule>();
            march.DoKnockback(boomboxForce);
        }
    }

    private IEnumerator BlinkDamage()
    {
        blinkingHealth = true;
        var ogTextColor = points.color;

        currentValue.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        currentValue.color = ogTextColor;
        blinkingHealth = false;
    }

    private void SetHealthValue()
    {
        currentValue.text = currentHealth.ToString();
    }

    public void RaiseMaxHealth(int value)
    {
        maxHealth += value;

        currentHealth += value;

        SetHealthValue();
    }

    void Update()
    {
        fillMat.SetFloat("_Fill", currentExperience / (float) requiredExperience);


        //Editor only
#if UNITY_EDITOR
        if(Input.GetKey(KeyCode.LeftControl))
        {
            if(Input.GetKeyDown(KeyCode.I)) 
            {
                buildPoints +=3;
                points.text = buildPoints.ToString() + " $";
            }
        }
#endif
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(boomboxPoint.position, boomboxSize);
    }
}
