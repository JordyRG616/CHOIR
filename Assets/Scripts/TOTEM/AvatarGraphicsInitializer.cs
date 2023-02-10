using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AvatarGraphicsInitializer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer body, overlay, hair;
    [SerializeField] private List<Light2D> lights;
    [SerializeField] private Material tankMaterial;
    [ColorUsage(true, true)] [SerializeField] private Color color;
    [Header("Assets")]
    [SerializeField] List<BodyGraphics> bodies;
    [SerializeField] List<HairGraphics> hairs;


    private void Start()
    {
        if(TotemManager.Main != null) DrawAvatar(TotemManager.Main.selectedAvatar);
    }

    public void DrawAvatar(AvatarData data)
    {
        var b_grph = bodies.Find(x => x.race == data.race);
        var h_grph = hairs.Find(x => x.HairStyle == data.hairStyle);

        body.sprite = b_grph.body;
        overlay.sprite = b_grph.overlay;
        hair.sprite = h_grph.hair;

        tankMaterial.SetColor("_Color", data.primaryColor * color);

        lights.ForEach(x => x.color = data.primaryColor);
    }
}
