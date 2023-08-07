using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carriage : Enemy
{
    public Enemy payload;

    public override void InitialStats()
    {
        this.gameObject.SetActive(true);
        this.health = 8;
        this.speed = Random.Range(75f, 125f);
    }

    public override IEnumerator TakeDamage()
    {
        health--;
        if (health == 0)
        {
            audiosource.PlayOneShot(clips[1]);
            Enemy enemy1 = Instantiate(payload, this.transform.parent);
            enemy1.StartingPosition(new Vector2(transform.localPosition.x, transform.localPosition.y+200));

            Enemy enemy2 = Instantiate(payload, this.transform.parent);
            enemy2.StartingPosition(new Vector2(transform.localPosition.x, transform.localPosition.y));

            Enemy enemy3 = Instantiate(payload, this.transform.parent);
            enemy3.StartingPosition(new Vector2(transform.localPosition.x, transform.localPosition.y - 200));

            Destroy(this.gameObject);
        }
        else
        {
            audiosource.PlayOneShot(clips[0]);
            damaged = true;
            yield return OnDamageEffect();
            damaged = false;
        }
    }
}
