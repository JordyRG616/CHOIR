using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CurveEvaluator : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    public int[] exps = new int[30];

    void Update()
    {
        for(int i = 0; i < exps.Length; i++)
        {
            exps[i] = Mathf.RoundToInt(curve.Evaluate(i));
        }
    }
}
