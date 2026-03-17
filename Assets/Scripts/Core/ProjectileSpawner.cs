using UnityEngine;
using System.Collections;

public class ProjectileSpawner : MonoBehaviour
{
    public Transform FirePoint;
    public GameObject ProjectilePrefab;
    public Animator Animator;
    public TurnManager turnManager;

    private float minForce = 5f;
    private float maxForce = 25f;
    private float forceChanging = 5f;

    private float currForce;
    private bool forceIncreasing = true;
    public bool canShoot = true;

    void Start()
    {
        currForce = minForce;
    }

    private void Update()
    {
        if (!canShoot) return;

        if(Input.GetButton("Fire1"))
        {
            if (forceIncreasing)
            {
                currForce += forceChanging * Time.deltaTime;
                if (currForce >= maxForce)
                {
                    currForce = maxForce;
                    forceIncreasing = false;
                }
            }
            else
            {
                currForce -= forceChanging * Time.deltaTime;
                if (currForce <= minForce)
                {
                    currForce = minForce;
                    forceIncreasing = true;
                }
            }
        }

        if (Input.GetButtonUp("Fire1") && canShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        canShoot = false;
        Animator.SetBool("isThrowing", true);

        yield return new WaitForSeconds(0.7f); // wait for the throwing animation to play

        GameObject projectile = Instantiate(ProjectilePrefab, FirePoint.position, FirePoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(FirePoint.right * currForce, ForceMode2D.Impulse);
        }

        Animator.SetBool("isThrowing", false);

        currForce= minForce;
        forceIncreasing = true;

        if(turnManager != null)
        {
            turnManager.NextTurn();
        }

    }
}
