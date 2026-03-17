using UnityEngine;

public class Projectile : MonoBehaviour
{
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
        Debug.Log(hitInfo.name);

        Animator hitAnimator = hitInfo.GetComponent<Animator>();
        if (hitAnimator != null)
        {
            hitAnimator.SetTrigger("isHit");
        }

        Destroy(gameObject);
    }
}