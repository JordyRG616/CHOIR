using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private float horizontalImpulse;
    [SerializeField] private float impulseValue;
    [SerializeField] private float torqueValue;
    [Space]
    [SerializeField] private Database Database;
    [Space]
    [SerializeField] private bool rewardWeapon = false;
    [SerializeField] private List<ChestUpgradeSelection> selections;

    private Animator anim;
    private Rigidbody2D body;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        var rdm = Random.Range(-horizontalImpulse, horizontalImpulse);
        var dir = Vector2.up + (Vector2.right * rdm);

        body.AddForce(dir * impulseValue, ForceMode2D.Impulse);
        body.AddTorque(torqueValue * -Mathf.Sign(rdm), ForceMode2D.Impulse);

        foreach (var selection in selections)
        {
            if (rewardWeapon) selection.ReceiveWeapon(Inventory.Main.GetRandomAvailableWeapon());
            else selection.ReceiveUpgrade(Database.GetRandomUpgrade(), Database.GetRandomMutation());
        }
    }

    private void OnMouseUp()
    {
        anim.SetTrigger("Open");
    }

    public void RemovePhysicsComponents()
    {
        Destroy(GetComponent<Collider2D>());
        Destroy(body);
        anim.enabled = false;
    }

    public void UnlockWeapon()
    {
        selections.ForEach(x => x.UnlockWeapon());
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
