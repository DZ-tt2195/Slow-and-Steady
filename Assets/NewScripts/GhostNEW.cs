using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class GhostNEW : EnemyNEW
{
    protected override void GhostBarrierTrigger(Collider2D collision)
    {
        if (collision.CompareTag("GhostBarrier"))
        {
            StartCoroutine(ShiftOpacity());
        }
    }

    IEnumerator ShiftOpacity()
    {
        float totalTime = 1f;
        if (defaultAlpha == 1)
        {
            float elapsedTime = 0f;
            while (elapsedTime < totalTime)
            {
                elapsedTime += Time.deltaTime;
                defaultAlpha = 1f - (elapsedTime / totalTime);
                yield return null;
            }
            defaultAlpha = 0;
        }
        else
        {
            float elapsedTime = 0f;
            while (elapsedTime < totalTime)
            {
                elapsedTime += Time.deltaTime;
                defaultAlpha = (elapsedTime / totalTime);
                yield return null;
            }
            defaultAlpha = 1;
        }
    }
}
