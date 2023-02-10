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
    [SerializeField] private float requiredExperience;
    [SerializeField] private AnimationCurve increment;
    //[SerializeField] private ParticleSystem gainExpVFX;
    [SerializeField] private StudioEventEmitter gainExpSFX;
    public int level { get; private set; } = 0;
    private int currentExperience;
    private int experienceHolder;
    [field: SerializeField] public int buildPoints { get; private set; }

    [Header("UI")]
    [SerializeField] private List<Vector2> barSizes;
    [SerializeField] private ParticleSystem levelVFX;
    [SerializeField] private List<Light2D> lights;

    [Header("Cash UI")]
    [SerializeField] private TextMeshProUGUI points;
    [SerializeField] private RectTransform expbar;
    [SerializeField] private Image cashIcon;

    public delegate void LevelUpEvent(int level);
    public LevelUpEvent onLevelUp;
    private bool blinkingCash;
    private bool blinkingHealth = false;
    private bool waitingLevelUp;

    private void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        currentExperience = 0;
        SetHealthValue();
        SetExpBarSize();
        points.text = buildPoints.ToString();
    }

    private void OnParticleCollision(GameObject other)
    {
        if(!waitingLevelUp) currentExperience += 1;
        else experienceHolder += 1;
        gainExpSFX.Play();

        if(currentExperience >= requiredExperience)
        {
            EnqueueLevelUp();
            currentExperience = 0;
        }

        //gainExpVFX.Play();
        SetExpBarSize();
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
        var ogImageColor = cashIcon.color;

        points.color = Color.red;
        cashIcon.color = Color.red;
        expbar.GetComponent<Image>().color = Color.red;

        yield return new WaitForSeconds(0.1f);

        points.color = ogTextColor;
        cashIcon.color = ogImageColor;
        expbar.GetComponent<Image>().color = ogImageColor;
        blinkingCash = false;
    }

    private void SetExpBarSize()
    {
        var percentage = currentExperience / requiredExperience;
        expbar.sizeDelta = Vector2.Lerp(barSizes[0], barSizes[1], percentage);
    }

    public void ExpendBuildPoints(int value)
    {
        buildPoints -= value;

        points.text = buildPoints.ToString();
    }

    private void EnqueueLevelUp()
    {
        waitingLevelUp = true;
        ActionMarker.Main.OnBeat += LevelUp;
    }

    private void LevelUp()
    {
        ActionMarker.Main.OnBeat -= LevelUp;

        requiredExperience += increment.Evaluate(level);
        level++;
        buildPoints++;
        levelVFX.Play();
        points.text = buildPoints.ToString();

        onLevelUp?.Invoke(level);

        waitingLevelUp = false;
        currentExperience += experienceHolder;
        experienceHolder = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<EnemyDamageModule>(out var damageController))
        {

            currentHealth -= damageController.damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                EndGameLog.Main.TriggerEndgame(false);
            }

            SetHealthValue();
            if (!blinkingHealth) StartCoroutine(BlinkDamage());
            collision.GetComponent<EnemyHealthModule>().Die(false);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        SetHealthValue();
    }

    private IEnumerator BlinkDamage()
    {
        blinkingHealth = true;
        var ogTextColor = points.color;
        var OgLightColor = lights[0].color;

        currentValue.color = Color.red;
        lights.ForEach(x => x.color = Color.red);

        yield return new WaitForSeconds(0.1f);

        currentValue.color = ogTextColor;
        lights.ForEach(x => x.color = OgLightColor);
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
}
