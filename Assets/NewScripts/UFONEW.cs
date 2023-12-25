using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFONEW : EnemyNEW
{
    protected override IEnumerator DamageEffect()
    {
        this.transform.localPosition = new Vector2(transform.localPosition.x - 2f, Random.Range(-3.5f, 3.5f));
        yield return EnemyNEW.damageWait;
    }
}
