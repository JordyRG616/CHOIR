using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraTileBackground : MonoBehaviour
{
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Image image;

    public void Activate()
    {
        image.overrideSprite = null;
    }

    public void Deactivate()
    {
        image.overrideSprite = inactiveSprite;
    }
}
