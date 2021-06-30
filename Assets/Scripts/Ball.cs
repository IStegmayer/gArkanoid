using System;
using UnityEngine;

public class Ball: MonoBehaviour
{
    public static event Action<Ball> OnBallDeath;
    private Rigidbody2D rb;

    public void Die()
    {
        OnBallDeath?.Invoke(this);
        Destroy(gameObject, 1);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        AudioManager.Instance.Play("playerHit");
        float y = rb.velocity.y;
        if (y < 5f && y > -5f)
        {
            if (y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 5f);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, -5f);
            }
        }
    }
}
