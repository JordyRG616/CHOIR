using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenBorder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Vector2 direction;
    private CameraManager cameraManager;
    private bool inside;


    void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        inside = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inside = false;
    }

    void Update()
    {
        if(inside)
        {
            cameraManager.MoveCamera(direction);
        }
    }
}
