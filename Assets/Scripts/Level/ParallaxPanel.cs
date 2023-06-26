using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxPanel : MonoBehaviour
{
    [SerializeField] private Vector2 boundaries;
    [SerializeField] private float scrollSpeed;

    private CameraManager cameraManager;

    void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
    }

    void Update()
    {
        if(cameraManager != null)
        {
            SetPanelPosition(cameraManager.NormalizedPosition);
        }
    }

    private void SetPanelPosition(Vector2 cameraPosition)
    {
        var posX = Mathf.Lerp(-boundaries.x, boundaries.x, Mathf.Clamp01(cameraPosition.x * scrollSpeed));
        var posY = Mathf.Lerp(-boundaries.y, boundaries.y, Mathf.Clamp01(cameraPosition.y * scrollSpeed));

        transform.localPosition = new Vector3(posX, posY, transform.localPosition.z);
    }
}
