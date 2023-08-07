using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 targetPosition;
    public float speed;

    private void FixedUpdate()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Barrier"))
        {
            GameObject.Find("Generator").GetComponent<Generator>().missedbullets++;
            Destroy(this.gameObject);
        }
        if (collision.gameObject.GetComponent<Enemy>() != null)
            Destroy(this.gameObject);
    }

}
