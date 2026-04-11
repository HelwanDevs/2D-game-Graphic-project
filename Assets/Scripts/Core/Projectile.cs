using UnityEngine;

public class Projectile : MonoBehaviour
{


    public float damage = 10f;
    private Rigidbody2D rb;

    public int shooterPlayer;



    private bool hasHit = false;
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
            hasHit = true;
            AudioManager.instance.PlaySFX(AudioManager.instance.hit);

            if (hitInfo.CompareTag("Player 1 (Hager)"))
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.fristPlayerHit);
            }
            else if (hitInfo.CompareTag("Player 2 (Mariam)"))
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.secondPlayerHit);
            }

            damageable.TakeDamage(100);

            Animator hitAnimator = hitInfo.GetComponent<Animator>();
            if (hitAnimator != null)
                hitAnimator.SetTrigger("isHit");
            Destroy(gameObject);
            return;
        }

        MissHitSounds();


    }


    void MissHitSounds()
    {
        if (shooterPlayer !=1)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.firstPlayerNotHit);

            Animator anim = TurnManager.Instance.player1.GetComponent<Animator>();
            if (anim != null)
                anim.SetTrigger("isMissing");
        }
        else
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.secondPlayerNotHit);
            Animator anim = TurnManager.Instance.player2.GetComponent<Animator>();
            if (anim != null)
                anim.SetTrigger("isMissing");

        }

        Destroy(gameObject);
    }
}