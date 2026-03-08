using UnityEngine;

/*public class ProjectileSpawner : MonoBehaviour
{
   public Transform Firepoint;
   public GameObject ProjectilePrefab;

    public bool canShoot = true; // can help into turn manager 

    void Update()
    {
        if (canShoot && Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        // shotting logic(we want to spawn a projectile at the firepoint)

        Instantiate(ProjectilePrefab, Firepoint.position, Firepoint.rotation);
    }

 
}*/

/*
 * public class ProjectileSpawner : MonoBehaviour
{
    public Transform Firepoint;
    public GameObject ProjectilePrefab;
    public float lanchForce = 15f;

    public bool canShoot = true; // can help into turn manager 

    void Update()
    {
        if (canShoot && Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject projectileInstance = Instantiate(ProjectilePrefab,Firepoint.position,Firepoint.rotation);

        Rigidbody2D rb = projectileInstance.GetComponent<Rigidbody2D>();

        rb.AddForce(Firepoint.right * lanchForce, ForceMode2D.Impulse);
    }

}

*/



// this is fully an AI code , i will work on it to make it better and more efficient but for now this is the best i can do with the time i have عايزة انام 
public class ProjectileSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform FirePoint;              // Where the projectile spawns
    public GameObject ProjectilePrefab;      // Assign Projectile_P1 or Projectile_P2 prefab

    [Header("Launch Settings")]
    public float minForce = 5f;             // Minimum charge force
    public float maxForce = 25f;            // Maximum charge force
    public float chargeSpeed = 15f;         // How fast force increases while holding

    [Header("Projectile Cycling")]
    private int nextProjectileChild = 0;    // 0 = Laptop, 1 = Chair

    [Header("Turn Control")]
    public bool canShoot = true;            // Controlled by Turn Manager

    private float currentForce;

    void Start()
    {
        currentForce = minForce;
    }

    void Update()
    {
        if (!canShoot) return;

        // 1️⃣ Charging force while holding Fire1
        if (Input.GetButton("Fire1"))
        {
            currentForce += chargeSpeed * Time.deltaTime;
            currentForce = Mathf.Clamp(currentForce, minForce, maxForce);
        }

        // 2️⃣ Shoot on release
        if (Input.GetButtonUp("Fire1"))
        {
            Shoot();
            currentForce = minForce; // reset for next shot
        }
    }

    private void Shoot()
    {
        // Spawn projectile at FirePoint
        GameObject projectile = Instantiate(ProjectilePrefab, FirePoint.position, FirePoint.rotation);

        // Activate only the correct child (Laptop / Chair)
        for (int i = 0; i < projectile.transform.childCount; i++)
        {
            projectile.transform.GetChild(i).gameObject.SetActive(i == nextProjectileChild);
        }

        // Apply force to parent Rigidbody2D
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(FirePoint.right * currentForce, ForceMode2D.Impulse);
        }

        // Cycle to next projectile type
        nextProjectileChild = (nextProjectileChild + 1) % projectile.transform.childCount;
    }
}