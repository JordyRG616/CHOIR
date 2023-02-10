using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using TotemEntities.DNA;

public class AvatarBox : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Vector2 originalSize, selectedSize;
    [SerializeField] private Color originalColor, hoverColor;
    [SerializeField] private TextMeshProUGUI specimenID, raceName;
    private RectTransform rect;
    private Image frame;
    private bool selected = false;

    [SerializeField] private AvatarData avatarData;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        frame = GetComponent<Image>();
    }

    public void ReceiveAvatar(TotemDNADefaultAvatar avatar)
    {
        avatarData = new AvatarData(avatar);
        specimenID.text = avatarData.name;
        raceName.text = avatarData.race.ToString();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        selected = !selected;

        if (selected)
        {
            foreach (var box in FindObjectsOfType<AvatarBox>())
            {
                box.selected = false;
                selected = true;
                box.SetUnselected();
                SetSelected();
                AssetBrowserManager.Main.DrawAvatar(avatarData);
            }
        }
        else
        {
            SetUnselected();
            AssetBrowserManager.Main.ClearAvatar();
        }
    }

    private void SetUnselected()
    {
        rect.sizeDelta = originalSize;
        frame.color = originalColor;
    }

    private void SetSelected()
    {
        rect.sizeDelta = selectedSize;
        frame.color = hoverColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        frame.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (selected) return;

        frame.color = originalColor;
    }
}

[System.Serializable]
public class AvatarData
{
    public string name;
    public AvatarRace race;
    public string hairStyle;
    public Color bodyColor;
    public Color hairColor;
    public Color primaryColor;

    public AvatarData(TotemDNADefaultAvatar avatar)
    {
        var _n = ColorUtility.ToHtmlStringRGB(avatar.secondary_color).Substring(1, 3);
        name = "Specimen " + _n;

        if(avatar.body_strength)
        {
            if (avatar.body_type) race = AvatarRace.Gikkak;
            else race = AvatarRace.Dhevo;
        } else
        {
            if (avatar.body_type) race = AvatarRace.Ecex;
            else race = AvatarRace.Aerni;
        }

        hairStyle = avatar.hair_styles;
        ColorUtility.TryParseHtmlString(avatar.human_skin_color, out bodyColor);
        ColorUtility.TryParseHtmlString(avatar.human_hair_color, out hairColor);
        primaryColor = avatar.primary_color;
    }

    public enum AvatarRace { Dhevo, Aerni, Ecex, Gikkak}
}