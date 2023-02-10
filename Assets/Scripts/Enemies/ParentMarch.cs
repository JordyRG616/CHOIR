using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentMarch : EnemyMarchModule
{
    [SerializeField] private List<EnemyMarchModule> childrenMarch;

    public override void SetDirection(int direction)
    {
        childrenMarch.ForEach(x => x.SetDirection(direction));
    }
}
