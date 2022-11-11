using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using System.Linq;

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
    [SerializeField] private TextMeshProUGUI currentValue, maxValue;
    [SerializeField] private Gradient healthColors;
    public int currentHealth { get; private set; }

    [Header("Experience Information")]
    [SerializeField] private float requiredExperience;
    [SerializeField] private AnimationCurve increment;
    [SerializeField] private ParticleSystem gainExpVFX;
    [SerializeField] private StudioEventEmitter gainExpSFX;
    public int level { get; private set; } = 0;
    private float currentExperience;

    [Header("Levels List")]
    [SerializeField] private List<int> patternUpgradeLevels;
    [SerializeField] private List<int> bossLevels;
    [Header("UI")]
    [SerializeField] private RectTransform expbar;
    [SerializeField] private Vector2 sizeBoundaries;
    [SerializeField] private Image rewardIcon;
    [SerializeField] private Sprite patternUpgradeIcon;
    [SerializeField] private TextMeshProUGUI levelUI;

    [Header("Final Boss")]
    [SerializeField] private GameObject leftMass;
    [SerializeField] private GameObject rightMass;

    private void Start()
    {
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        currentExperience = 0;
        maxValue.text = maxHealth.ToString();
        SetHealthValue();
        SetExpBarSize();
    }

    private void OnParticleCollision(GameObject other)
    {
        currentExperience += 1;
        gainExpSFX.Play();

        TutorialManager.Main.DoTutorialStep("Leveling Up", true);

        if(currentExperience >= requiredExperience)
        {
            if (patternUpgradeLevels.Contains(level)) PatternUpgradeManager.Main.InitiateSelection();
            else RewardManager.Main.OpenReward(RewardManager.Main.defaultCards);

            SetNextLevelInfo();
            currentExperience = 0;
            if (bossLevels.Contains(level)) ToBossLevel();
            if (level == 50) StartFinalBoss();

            Time.timeScale = 0;
        }

        gainExpVFX.Play();
        SetExpBarSize();
    }

    private void StartFinalBoss()
    {
        foreach (var enemy in FindObjectsOfType<EnemyHealthController>())
        {
            enemy.Die(false);
        }

        foreach (var spawner in FindObjectsOfType<EnemySpawner>())
        {
            spawner.active = false;
        }

        rightMass.SetActive(true);
        leftMass.SetActive(true);

        cameraManager.GoToFinalCamera();
    }

    private void ToBossLevel()
    {
        var direction = (level == bossLevels[0]) ? Vector3.left : Vector3.right;
        cameraManager.GoToBossCamera(direction);

        foreach (var enemy in FindObjectsOfType<EnemyHealthController>())
        {
            enemy.Die(false);
        }

        var position = (level == bossLevels[0]) ? EnemySpawner.SpawnerPosition.Right : EnemySpawner.SpawnerPosition.Left;
        var spawners = FindObjectsOfType<EnemySpawner>().ToList();
        var spawner = spawners.Find(x => x.position == position);
        spawner.active = false;

        var order = (level == bossLevels[0]) ? 0 : 1;
        var boss = FindObjectsOfType<BossEnemy>(true).ToList().Find(x => x.bossOrder == order);
        boss.gameObject.SetActive(true);
    }

    private void SetExpBarSize()
    {
        var percentage = currentExperience / requiredExperience;
        var sizeX = sizeBoundaries.x + ((sizeBoundaries.y - sizeBoundaries.x) * percentage);
        expbar.sizeDelta = new Vector2(sizeX, expbar.sizeDelta.y);
    }

    private void SetNextLevelInfo()
    {
        requiredExperience += increment.Evaluate(level);
        level++;

        levelUI.text = "LVL " + level;
        if (patternUpgradeLevels.Contains(level)) rewardIcon.overrideSprite = patternUpgradeIcon;
        else rewardIcon.overrideSprite = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<EnemyDamageController>(out var damageController))
        {

            currentHealth -= damageController.damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                EndGameLog.Main.TriggerEndgame(false);
            }
            anim.SetTrigger("Damaged");
            cameraManager.DoShake();
            SetHealthValue();
            collision.GetComponent<EnemyHealthController>().Die(false);
            TutorialManager.Main.DoTutorialStep("Taking Damage", true);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        SetHealthValue();
    }

    private void SetHealthValue()
    {
        currentValue.text = currentHealth.ToString();
        currentValue.color = healthColors.Evaluate(1 - ((float)currentHealth / maxHealth));
    }

    public void RaiseMaxHealth(int value)
    {
        maxHealth += value;
        maxValue.text = maxHealth.ToString();

        currentHealth += value;

        SetHealthValue();
    }
}
