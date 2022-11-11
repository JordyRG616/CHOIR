using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public int bossOrder;
    [SerializeField] private GameObject associatedSpawner;
    private Vector3 originalSpawnerPosition;
    private EnemyHealthController healthController;

    [Header("UI")]
    [SerializeField] private RectTransform bar;
    [SerializeField] private GameObject defeatedBox;
    [SerializeField] private float maxBarSize;
    [SerializeField] private Animator UiAnimator;

    private void Awake()
    {
        healthController = GetComponent<EnemyHealthController>();
        healthController.onEnemyDeath += BossDeath;
        healthController.onDamageTaken += UpdateHealthbar;

        originalSpawnerPosition = associatedSpawner.transform.position;
        associatedSpawner.transform.position = transform.position;

        UiAnimator.SetTrigger("Open");
        UpdateHealthbar(1);
    }

    private void BossDeath(EnemyHealthController healthController, bool destroy)
    {
        FindObjectOfType<CameraManager>().GoToBossCamera(Vector2.zero);

        foreach (var spawner in FindObjectsOfType<EnemySpawner>())
        {
            spawner.active = true;
        }

        defeatedBox.SetActive(true);
        associatedSpawner.transform.position = originalSpawnerPosition;
        Destroy(gameObject);
    }

    private void UpdateHealthbar(float percentage)
    {
        bar.sizeDelta = new Vector2(maxBarSize * percentage, bar.sizeDelta.y);
    }
}
