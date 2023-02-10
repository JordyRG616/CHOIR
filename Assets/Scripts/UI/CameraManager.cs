using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float radius, speed, duration;

    [Header("Boss Transition")]
    [SerializeField] private float bossCameraPosition;
    [SerializeField] private float transitionSpeed;

    private WaitForSeconds waitTime = new WaitForSeconds(0.1f);
    private Vector3 origin = new Vector3(0, 0, -10);
    private WaitForSeconds wait = new WaitForSeconds(0.01f);
    private bool onBossCamera;

    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private float cameraSpeed;
    private Vector2 cameraBoundaries;

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
        if (onBossCamera) return;
        StopAllCoroutines();
        origin = transform.position;
        transform.position = GetRandomPosition();
        StartCoroutine(ReturnToOrigin());
    }

    private Vector3 GetRandomPosition()
    {
        var position = Random.onUnitSphere;
        position *= radius;
        position.z = -10;

        return position;        
    }

    private IEnumerator ReturnToOrigin()
    {
        yield return waitTime;

        transform.position = origin;
    }

    public void GoToBossCamera(Vector2 direction)
    {
        StartCoroutine(BossTransition(direction));
        if (direction == Vector2.zero) onBossCamera = false;
        else onBossCamera = true;
    }

    public void GoToFinalCamera()
    {
        StartCoroutine(FinalBossTransition());
    }

    private IEnumerator BossTransition(Vector2 direction)
    {
        float step = 0;
        var originalPosition = transform.position;
        Vector3 targetPosition = direction * bossCameraPosition;
        targetPosition.z = -25;

        while(step <= 1)
        {
            var position = Vector3.Lerp(originalPosition, targetPosition, step);
            position.z = -25;

            transform.position = position;

            step += transitionSpeed;
            yield return wait;
        }

        transform.position = targetPosition;
    }

    private IEnumerator FinalBossTransition()
    {
        float step = 0;

        while (step <= 1)
        {
            Camera.main.orthographicSize = Mathf.Lerp(11.25f, 15f, step);

            step += 0.01f;
            yield return wait;
        }
    }

    private void Update()
    {
        MoveCamera();

        if(Input.GetKeyDown(KeyCode.F))
        {
            transform.position = origin;
        }
    }

    private void MoveCamera()
    {
        posX += Time.deltaTime * cameraSpeed * Input.GetAxis("Horizontal");
        posY += Time.deltaTime * cameraSpeed * Input.GetAxis("Vertical");

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
