using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeNEW : AbilityNEW
{
    protected override IEnumerator AbilityEffect()
    {
        for (int i = 0; i < 2; i++)
        {
            EnemyNEW[] listofenemies = FindObjectsOfType<EnemyNEW>();
            foreach (EnemyNEW enemy in listofenemies)
            {
                StartCoroutine(enemy.TakeDamage());
                AudioManager.instance.PlaySound(sound, 0.4f);
            }
            yield return new WaitForSeconds(1.5f);
        }
    }

}
