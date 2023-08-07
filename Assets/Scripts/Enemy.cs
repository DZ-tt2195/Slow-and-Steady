using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int health;
    public float speed;
    public Image sr;
    public Color startingColor;
    public AudioSource audiosource;

    public AudioClip[] clips = new AudioClip[2];
    public bool frozen = false;
    public bool damaged = false;

    public void Start()
    {
        audiosource = GameObject.Find("Player").GetComponent<AudioSource>();
        sr = this.GetComponent<Image>();
        InitialStats();
        startingColor = sr.color;
    }

    public void StartingPosition(Vector2 start)
    {
        transform.localPosition = start;
    }

    public virtual void InitialStats()
    {
        this.gameObject.SetActive(true);
        this.health = 3;
        this.speed = Random.Range(50f, 100f);
    }

    public void Update()
    {
        if (damaged)
            sr.color = Color.red;
        else if (frozen)
            sr.color = Color.blue;
        else
            sr.color = startingColor;
    }

    public virtual IEnumerator TakeDamage()
    {
        health--;
        if (health == 0)
        {
            audiosource.PlayOneShot(clips[1]);
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

    public virtual IEnumerator OnDamageEffect()
    {
        yield return new WaitForSeconds(0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            StartCoroutine(TakeDamage());
        }

        if (collision.name == "Generator")
            collision.GetComponent<Generator>().GameOver();
    }

    void FixedUpdate()
    {
        transform.Translate((Vector2.left * Time.deltaTime) * speed);
    }
}
