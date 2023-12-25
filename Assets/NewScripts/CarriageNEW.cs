using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarriageNEW : EnemyNEW
{
    [SerializeField] EnemyNEW payload;

    protected override IEnumerator DeathEffect()
    {
        EnemyNEW enemy1 = Instantiate(payload, this.transform.parent);
        enemy1.transform.localPosition = (new Vector2(transform.localPosition.x, transform.localPosition.y + 1.25f));

        EnemyNEW enemy2 = Instantiate(payload, this.transform.parent);
        enemy2.transform.localPosition = (new Vector2(transform.localPosition.x, transform.localPosition.y));

        EnemyNEW enemy3 = Instantiate(payload, this.transform.parent);
        enemy3.transform.localPosition = (new Vector2(transform.localPosition.x, transform.localPosition.y - 1.25f));

        yield return null;
    }
}
