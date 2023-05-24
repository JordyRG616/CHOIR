using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingDamage : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject critPanel;
    private FloatingDamagePooler pooler;
    private int currentDamage;
    private int targetDamage;
    private float counter;
    private float duration = 0.2f;
    private bool counting;
    private WaitForSeconds updateTime = new WaitForSeconds(0.05f);

    public void Pop(int damageTaken, FloatingDamagePooler pooler, bool crit)
    {
        currentDamage = damageTaken;
        text.text = currentDamage.ToString("00");
        gameObject.SetActive(true);
        counting = true;
        critPanel.SetActive(crit);

        this.pooler = pooler;
    }

    public void ReceiveDamage(int damage, bool crit)
    {
        targetDamage += damage;
        counter = 0;
        critPanel.SetActive(crit);
        StartCoroutine(UpdateDamage());
    }

    private IEnumerator UpdateDamage()
    {
        StopAllCoroutines();
        while(currentDamage < targetDamage)
        {
            currentDamage++;
            text.text = currentDamage.ToString("00");
            yield return updateTime;
        }
    }

    public void SetUnavailable()
    {
        pooler.availablePop = null;
        anim.SetTrigger("Fade");
        counting = false;
        counter = 0;
    }

    void Update()
    {
        if (counting)
        {
            counter += Time.deltaTime;
    
            if(counter >= duration)
            {
                SetUnavailable();
            }
        }

        transform.position = pooler.transform.position;
    }
}
