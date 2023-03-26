using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class EnemyBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image sprite;
    [SerializeField] private TextMeshProUGUI healthValue;
    [SerializeField] private GameObject traitBox;
    [SerializeField] private GameObject descBox;
    [SerializeField] private Image traitIcon;
    [SerializeField] private TextMeshProUGUI traitName;
    [SerializeField] private TextMeshProUGUI traitDescription;


    public void ReceiveEnemy(EnemyManager enemy)
    {
        sprite.sprite = enemy.sprite;
        healthValue.text = enemy.GetDeafultHealth();

        if(enemy.hasTrait)
        {
            traitBox.SetActive(true);
            traitIcon.sprite = enemy.trait.icon;
            traitName.text = enemy.trait.name;
            traitDescription.text = enemy.trait.description;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        descBox.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descBox.SetActive(false);
    }
}
