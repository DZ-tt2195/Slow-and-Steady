using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miner : Enemy
{
    public override void InitialStats()
    {
        this.gameObject.SetActive(true);
        this.health = 4;
        this.speed = Random.Range(125f, 175f);
    }

    public override IEnumerator OnDamageEffect()
    {
        sr.color = Color.red;
        this.transform.localPosition = new Vector2(transform.localPosition.x - 250, Random.Range(-350f, 350f));
        yield return new WaitForSeconds(0.5f);
        sr.color = startingColor;
    }

}
