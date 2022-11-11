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
}
