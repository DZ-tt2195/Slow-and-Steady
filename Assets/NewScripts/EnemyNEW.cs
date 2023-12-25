using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

public class EnemyNEW : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] float slowest;
    [SerializeField] float fastest;
    float actualSpeed;

    bool frozen = false;
    bool takingDamage = false;
    protected static WaitForSeconds damageWait;

    protected SpriteRenderer sprite;
    protected float defaultAlpha = 1;
    Color startingColor;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        startingColor = sprite.color;
        damageWait ??= new WaitForSeconds(0.8f);
        actualSpeed = Random.Range(slowest, fastest);
    }

    void Update()
    {
        if (takingDamage)
        {
            sprite.color = Color.red;
            sprite.SetAlpha(1);
        }
        else if (frozen)
        {
            sprite.color = Color.blue;
            sprite.SetAlpha(1);
        }
        else
        {
            sprite.color = startingColor;
            sprite.SetAlpha(defaultAlpha);
        }
    }

    void FixedUpdate()
    {
        transform.Translate((Vector2.left * Time.deltaTime) * actualSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(TakeDamage());
        }

        if (collision.gameObject == GeneratorNEW.instance.gameObject)
        {
            GeneratorNEW.instance.GameOver();
        }

        GhostBarrierTrigger(collision);
    }

    protected virtual void GhostBarrierTrigger(Collider2D collision)
    {
    }

    IEnumerator TakeDamage()
    {
        health--;
        if (health == 0)
        {
            yield return DeathEffect();
            Destroy(this.gameObject);
        }
        else
        {
            takingDamage = true;
            yield return DamageEffect();
            takingDamage = false;
        }
    }

    protected virtual IEnumerator DamageEffect()
    {
        yield return damageWait;
    }

    protected virtual IEnumerator DeathEffect()
    {
        yield return null;
    }
}
