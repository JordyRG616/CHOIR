using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : MonoBehaviour
{
    [SerializeField] private AnimationCurve launchCurve;
    [SerializeField] private float lifetime, wiggleAmplitude, horizontalSpeed;
    private WaitForSeconds wait = new WaitForSeconds(0.01f);


    private IEnumerator Start()
    {
        GetComponentInChildren<WeaponDamageDealer>().SetWeapon(GetComponentInParent<WeaponBase>());

        yield return StartCoroutine(Launch());

        horizontalSpeed *= Mathf.Sign(transform.parent.position.x);

        yield return StartCoroutine(Wiggle());

        Destroy(gameObject);
    }

    private IEnumerator Launch()
    {
        float step = 0;

        while (step <= 1)
        {
            transform.position += new Vector3(0, launchCurve.Evaluate(step));

            step += 0.01f;
            yield return wait;
        }
    }

    private IEnumerator Wiggle()
    {
        float count = 0;

        while (count <= lifetime)
        {
            transform.position += new Vector3(horizontalSpeed * Time.deltaTime, -Mathf.Sin(count) * wiggleAmplitude);

            count += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
