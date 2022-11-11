using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentMarch : EnemyMarch
{
    [SerializeField] private List<EnemyMarch> childrenMarch;

    public override void SetDirection(int direction)
    {
        childrenMarch.ForEach(x => x.SetDirection(direction));
    }
}
