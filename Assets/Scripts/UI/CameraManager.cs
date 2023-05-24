using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float radius;

    private WaitForSeconds waitTime = new WaitForSeconds(0.075f);
    private bool shaking;
    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float cameraMouseSpeed;
    private Vector2 cameraBoundaries;
    private Vector2 mousePos;

    private float _px = 0.5f;
    private float posX
    {
        get => _px;
        set
        {
            if (value < 0) _px = 0;
            else if (value > 1) _px = 1;
            else _px = value;
        }
    }

    private float _py = 0.5f;
    private float posY
    {
        get => _py;
        set
        {
            if (value < 0) _py = 0;
            else if (value > 1) _py = 1;
            else _py = value;
        }
    }
    private Camera cam;


    private void Start()
    {
        cam = GetComponent<Camera>();
        SetCameraBoundaries(levelGenerator.loadedData.LevelSize);
    }

    public void DoShake()
    {
        StopAllCoroutines();
        shaking = true;
        transform.position += GetRandomPosition();
        StartCoroutine(ReturnToOrigin());
    }

    private Vector3 GetRandomPosition()
    {
        var position = Random.onUnitSphere;
        position *= radius;
        position.z = 0;

        return position;        
    }

    private IEnumerator ReturnToOrigin()
    {
        yield return waitTime;
        transform.position += GetRandomPosition();
        yield return waitTime;
        shaking = false;
    }


    private void Update()
    {
        if(!shaking) MoveCamera();
    }

    private void MoveCamera()
    {
        if(Input.GetMouseButtonDown(2)) mousePos = Input.mousePosition;

        if(Input.GetMouseButton(2))
        {
            var diff = (Vector2)Input.mousePosition - mousePos;

            posX += Time.deltaTime * cameraMouseSpeed * diff.x;
            posY += Time.deltaTime * cameraMouseSpeed * diff.y;
        } else
        {
            posX += Time.deltaTime * cameraSpeed * Input.GetAxis("Horizontal");
            posY += Time.deltaTime * cameraSpeed * Input.GetAxis("Vertical");
        }

        var _x = Mathf.Lerp(-cameraBoundaries.x, cameraBoundaries.x, posX);
        var _y = Mathf.Lerp(-cameraBoundaries.y, cameraBoundaries.y, posY);

        transform.position = new Vector3(_x, _y, -10);
    }

    public void MoveCamera(Vector2 direction)
    {
        posX += Time.deltaTime * cameraSpeed * direction.x;
        posY += Time.deltaTime * cameraSpeed * direction.y;

        var _x = Mathf.Lerp(-cameraBoundaries.x, cameraBoundaries.x, posX);
        var _y = Mathf.Lerp(-cameraBoundaries.y, cameraBoundaries.y, posY);

        transform.position = new Vector3(_x, _y, -10);
    }

    public void SetCameraBoundaries(Vector2 levelSize)
    {
        levelSize /= 32;
        var diff = levelSize - new Vector2(cam.orthographicSize * 16 / 9, cam.orthographicSize);
        cameraBoundaries = diff;
    }
}
