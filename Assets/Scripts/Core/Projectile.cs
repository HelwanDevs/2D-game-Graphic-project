using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10f;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.AddForce(new Vector2(WindSystem.windForce, 0f), ForceMode2D.Force);
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        IDamageable damageable = hitInfo.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);

            Animator hitAnimator = hitInfo.GetComponent<Animator>();
            if (hitAnimator != null)
                hitAnimator.SetTrigger("isHit");
        }

        Destroy(gameObject);
    }
}