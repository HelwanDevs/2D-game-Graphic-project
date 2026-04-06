using UnityEngine;
using System.Collections;

public class ProjectileSpawner : MonoBehaviour
{
    public Transform FirePoint;
    public GameObject ProjectilePrefab;
    public Animator Animator;
    public TurnManager turnManager;

    public float minForce = 5f;
    public float maxForce = 25f;
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
        Debug.Log($"[{gameObject.name}] canShoot: {canShoot}");
        if (!canShoot) return;

        if (Input.GetButton("Fire1"))
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
            StartCoroutine(Shoot(FirePoint.right, currForce));  //changed
        }
    }

    //private IEnumerator Shoot()
    //{
    //    canShoot = false;
    //    Animator.SetBool("isThrowing", true);

    //    yield return new WaitForSeconds(0.7f); // wait for the throwing animation to play

    //    GameObject projectile = Instantiate(ProjectilePrefab, FirePoint.position, FirePoint.rotation);
    //    Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
    //    if (rb != null)
    //    {
    //        rb.AddForce(FirePoint.right * currForce, ForceMode2D.Impulse);
    //    }

    //    Animator.SetBool("isThrowing", false);

    //    currForce = minForce;
    //    forceIncreasing = true;

    //    if (turnManager != null)
    //    {
    //        turnManager.NextTurn();
    //    }

    //}

    private IEnumerator Shoot(Vector2 direction, float force)
    {
        canShoot = false;
        Animator.SetBool("isThrowing", true);
        yield return new WaitForSeconds(0.7f);

        GameObject projectile = Instantiate(ProjectilePrefab, FirePoint.position, FirePoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.AddForce(direction * force, ForceMode2D.Impulse);

        Animator.SetBool("isThrowing", false);
        currForce = minForce;
        forceIncreasing = true;

        if (turnManager != null)
            turnManager.NextTurn();
    }

    //public void ComputerShoot(Vector2 direction, float force)
    //{
    //    if (!canShoot) return;
    //    // rotate FirePoint to face the aimed direction
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    FirePoint.rotation = Quaternion.Euler(0f, 0f, angle);
    //    StartCoroutine(Shoot(FirePoint.right, force));
    //}

    public void ComputerShoot(Vector2 direction, float force)
    {
        if (!canShoot) return;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        FirePoint.rotation = Quaternion.Euler(0f, 0f, angle);
        Debug.Log($"[Computer] Shooting angle: {angle}, force: {force}");
        StartCoroutine(Shoot(FirePoint.right, force));
    }
}