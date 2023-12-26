using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeNEW : AbilityNEW
{
    protected override IEnumerator AbilityEffect()
    {
        AudioManager.instance.PlaySound(sound, 0.4f);
        EnemyNEW[] listofenemies = FindObjectsOfType<EnemyNEW>();
        foreach (EnemyNEW enemy in listofenemies)
            StartCoroutine(enemy.Freeze());
        yield return null;
    }
}