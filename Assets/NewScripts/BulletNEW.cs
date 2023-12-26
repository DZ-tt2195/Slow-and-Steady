using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletNEW : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    internal Vector2 targetPosition;
    Rigidbody2D rb;
    public bool active = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, bulletSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Barrier"))
        {
            GeneratorNEW.instance.missedBullets++;
            Destroy(this.gameObject);
        }
    }
}
