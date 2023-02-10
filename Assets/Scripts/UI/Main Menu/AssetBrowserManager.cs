using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TotemEntities.DNA;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class AssetBrowserManager : MonoBehaviour
{
    #region Main
    private static AssetBrowserManager _instance;
    public static AssetBrowserManager Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<AssetBrowserManager>();

            return _instance;
        }

    }
    #endregion

    [SerializeField] private RectTransform frame;
    [SerializeField] private AvatarBox modelBox;
    [ColorUsage(true, true)] [SerializeField] private Color color;

    [Header("Assets")]
    [SerializeField] List<BodyGraphics> bodies;
    [SerializeField] List<HairGraphics> hairs;

    [Header("AvatarGraphics")]
    [SerializeField] private Image body;
    [SerializeField] private Image overlay, hair;
    [SerializeField] private Material bodyMaterial, hairMaterial, tankMaterial;
    [SerializeField] private List<Light2D> lights;

    private float maxPos => Mathf.Clamp((frame.childCount - 4) * 80, 0, int.MaxValue);
    private float _p;
    private float pos
    {
        get => _p;
        set
        {
            if (value < 0) _p = 0;
            else if (value > maxPos) _p = maxPos;
            else _p = value;
        }
    }


    public void ReceiveNewAvatar(TotemDNADefaultAvatar avatar)
    {
        var box = Instantiate(modelBox, frame);
        box.ReceiveAvatar(avatar);
    }

    public void DrawAvatar(AvatarData data)
    {
        body.gameObject.SetActive(true);
        hair.gameObject.SetActive(true);
        overlay.gameObject.SetActive(true);


        var b_grph = bodies.Find(x => x.race == data.race);
        var h_grph = hairs.Find(x => x.HairStyle == data.hairStyle);

        body.sprite = b_grph.body;
        overlay.sprite = b_grph.overlay;
        overlay.color = data.primaryColor;
        hair.sprite = h_grph.hair;

        bodyMaterial.SetColor("_BodyColor", data.bodyColor);
        hairMaterial.SetColor("_BodyColor", data.hairColor);

        Color.RGBToHSV(data.primaryColor, out var h, out var s, out var v);
        data.primaryColor = Color.HSVToRGB(h, .8f, 1);

        tankMaterial.SetColor("_Color", data.primaryColor * color);

        lights.ForEach(x => x.color = data.primaryColor);

        TotemManager.Main.selectedAvatar = data;
    }

    private void Update()
    {
        if (frame.childCount > 0)
        {
            pos -= Input.mouseScrollDelta.y * 10;

            frame.anchoredPosition = new Vector2(0, pos);
        }
    }

    public void ClearAvatar()
    {
        body.gameObject.SetActive(false);
        hair.gameObject.SetActive(false);
        overlay.gameObject.SetActive(false);
    }
}

[System.Serializable]
public struct BodyGraphics
{
    public AvatarData.AvatarRace race;
    public Sprite body;
    public Sprite overlay;
}

[System.Serializable]
public struct HairGraphics
{
    public string HairStyle;
    public Sprite hair;
} 