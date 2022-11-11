using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnerInfo : MonoBehaviour
{
    [SerializeField] private List<InfoPanel> panels;
    [SerializeField] private Transform enemies;
    public ParticleSystem highlight;
    private Dictionary<SpawnerStat, float> values = new Dictionary<SpawnerStat, float>();

    private void Start()
    {
        values.Add(SpawnerStat.Frequency, 10);
        values.Add(SpawnerStat.Damage, 0);
        values.Add(SpawnerStat.Health, 0);
        values.Add(SpawnerStat.Speed, 0);
    }

    public void EnablePreview(SpawnerStat stat, float value)
    {
        GetPanel(stat).ReceivePreview(value);
        highlight.Play();
    }

    public void DisablePreview()
    {
        panels.ForEach(x => x.DeactivatePreview());
        highlight.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void ReceiveIncrement(SpawnerStat stat, float value)
    {
        values[stat] += value;
        GetPanel(stat).SetValue(values[stat]);
    }

    private InfoPanel GetPanel(SpawnerStat stat)
    {
        var panel = panels.Find(x => x.stat == stat);
        return panel;
    }

    public void SetEnemieInfo(List<(string name, float percentage)> info)
    {
        for (int i = 0; i < enemies.childCount; i++)
        {
            enemies.GetChild(i).gameObject.SetActive(false);
        }

        foreach (var item in info)
        {
            var panel = enemies.Find(item.name);
            panel.gameObject.SetActive(true);
            panel.Find("Value").GetComponent<TextMeshProUGUI>().text = (item.percentage * 100).ToString("0") + "%";
        }
    }
}

[System.Serializable]
public class InfoPanel
{
    public SpawnerStat stat;
    [SerializeField] private TextMeshProUGUI value, preview;
    public bool integerValue;
    public string preText, posText, previewPreText;

    public void SetValue(float newValue)
    {
        if (integerValue)
        {
            newValue = Mathf.Round(newValue);
        }

        var _v = integerValue ? newValue.ToString("0") : newValue.ToString("0.0");
        value.text = preText + _v + posText;

    }

    public void ReceivePreview(float previewValue)
    {
        if (integerValue)
        {
            previewValue = Mathf.Round(previewValue);
        }

        var _v = integerValue ? previewValue.ToString("0") : previewValue.ToString("0.0");
        preview.text = previewPreText + _v + posText;
        preview.gameObject.SetActive(true);
    }

    public void DeactivatePreview()
    {
        preview.gameObject.SetActive(false);
    }
}

public enum SpawnerStat { Frequency, Damage, Health, Speed}