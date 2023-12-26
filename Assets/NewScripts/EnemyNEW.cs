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

    [SerializeField] AudioClip damage;
    [SerializeField] AudioClip die;

    bool frozen = false;
    bool takingDamage = false;
    protected static WaitForSeconds damageWait;
    protected static WaitForSeconds freezeWait;

    protected SpriteRenderer sprite;
    protected float defaultAlpha = 1;
    Color startingColor;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        startingColor = sprite.color;
        damageWait ??= new WaitForSeconds(0.8f);
        freezeWait ??= new WaitForSeconds(5f);
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
        if (!frozen)
            transform.Translate((Vector2.left * Time.deltaTime) * actualSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            BulletNEW bullet = collision.GetComponent<BulletNEW>();
            if (bullet.active)
            {
                bullet.active = false;
                Destroy(collision.gameObject);
                StartCoroutine(TakeDamage());
            }
        }

        if (collision.gameObject == GeneratorNEW.instance.gameObject)
        {
            GeneratorNEW.instance.GameOver("You Lost.");
        }

        GhostBarrierTrigger(collision);
    }

    protected virtual void GhostBarrierTrigger(Collider2D collision)
    {
    }

    internal IEnumerator TakeDamage()
    {
        health--;
        if (health == 0)
        {
            AudioManager.instance.PlaySound(die, 0.3f);
            yield return DeathEffect();
            Destroy(this.gameObject);
        }
        else
        {
            AudioManager.instance.PlaySound(damage, 0.3f);
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

    internal IEnumerator Freeze()
    {
        frozen = true;
        yield return freezeWait;
        frozen = false;
    }
}
