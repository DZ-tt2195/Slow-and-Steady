using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy
{
    float opacity = 1;
    bool decreasing = true;

    public override void InitialStats()
    {
        this.gameObject.SetActive(true);
        this.health = 5;
        this.speed = Random.Range(125f, 225f);
    }

    public void Update()
    {
        if (damaged)
            sr.color = new Color(1, 0, 0, 1);
        else if (frozen)
            sr.color = new Color(0, 0, 1, 1);
        else
            sr.color = new Color(1, 1, 1, opacity);
    }

    IEnumerator ShiftOpacity()
    {
        yield return new WaitForSeconds(0.05f);
        if (decreasing)
        {
            opacity -= 0.05f;
            if (opacity > 0)
                yield return ShiftOpacity();
            else
                decreasing = false;
        }
        else
        {
            opacity += 0.05f;
            if (opacity < 1)
                yield return ShiftOpacity();
            else
                decreasing = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("GhostBarrier"))
        {
            StartCoroutine(ShiftOpacity());
        }
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(TakeDamage());
        }
        if (collision.name == "Generator")
            collision.GetComponent<Generator>().GameOver();
    }
}
