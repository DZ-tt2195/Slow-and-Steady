using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstNEW : AbilityNEW
{
    protected override IEnumerator AbilityEffect()
    {
        PlayerNEW.instance.energy += 25;
        AudioManager.instance.PlaySound(sound, 0.4f);
        yield return null;
    }
}
