using UnityEngine;
using System.Collections;
using Unity.Netcode;

public class ProjectileSpawner : NetworkBehaviour
{
    public Transform FirePoint;
    public GameObject ProjectilePrefab;
    public Animator Animator;
    public TurnManager turnManager;
    public ForceIndicator forceIndicator;

    public float minForce = 5f;
    public float maxForce = 25f;
    private float forceChanging = 5f;
    private float currForce;
    public bool canShoot = false;
    private bool forceIncreasing = true;




    void Start()
    {
        currForce = minForce;
    }

    private void Update()
    {
        Debug.Log($"{name} | canShoot: {canShoot} | IsOwner: {IsOwner} | currPlayer: {TurnManager.Instance.currPlayer.Value}");
        if (!canShoot) return;
        if (GameSettings.isNetworkMultiplayer && !IsOwner) return;
        int netId = gameObject.name.Contains("Player 1") ? 1 : 2;
        if (TurnManager.Instance.currPlayer.Value != netId) return;

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

            Debug.Log($"[{gameObject.name}] currForce: {currForce:F1}, increasing: {forceIncreasing}");

            if (forceIndicator != null)
                forceIndicator.SetForce(currForce, minForce, maxForce);
        }

        if (Input.GetButtonUp("Fire1") && canShoot)
        {
            if (GameSettings.isNetworkMultiplayer)
            {
                ShootRpc(FirePoint.right, currForce);//to ask host if can shoot
            }
            else
            {
                StartCoroutine(Shoot(FirePoint.right, currForce));
            }
            if (forceIndicator != null)
                forceIndicator.Hide();
        }
    }

    private IEnumerator Shoot(Vector2 direction, float force)
    {
        canShoot = false;
        Animator.SetBool("isThrowing", true);
        yield return new WaitForSeconds(0.7f);

        GameObject projectile = Instantiate(ProjectilePrefab, FirePoint.position, FirePoint.rotation);
        Projectile p = projectile.GetComponent<Projectile>();
        p.shooterPlayer = turnManager.currPlayer.Value;
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.AddForce(direction * force, ForceMode2D.Impulse);


        if (GameSettings.isNetworkMultiplayer)
        {
            projectile.GetComponent<NetworkObject>().Spawn();
        }
        AudioManager.instance.PlaySFX(AudioManager.instance.shooting);


        Animator.SetBool("isThrowing", false);
        currForce = minForce;

        if (turnManager != null)
            if (!GameSettings.isNetworkMultiplayer || IsServer)
            {
                turnManager.NextTurn();
            }
        currForce = minForce;
        forceIncreasing = true;
    }

    public void ComputerShoot(Vector2 direction, float force)
    {
        if (!canShoot) return;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        FirePoint.rotation = Quaternion.Euler(0f, 0f, angle);
        StartCoroutine(ComputerShootRoutine(direction, force));
    }

    IEnumerator ComputerShootRoutine(Vector2 direction, float force)
    {
        // gradually fill indicator
        float fillTime = 0.8f;
        float elapsed = 0f;
        while (elapsed < fillTime)
        {
            elapsed += Time.deltaTime;
            float current = Mathf.Lerp(minForce, force, elapsed / fillTime);
            if (forceIndicator != null)
                forceIndicator.SetForce(current, minForce, maxForce);
            yield return null;
        }

        // shoot after fill
        StartCoroutine(Shoot(direction, force));
        if (forceIndicator != null)
            forceIndicator.Hide();
    }

    void HideIndicator()
    {
        if (forceIndicator != null)
            forceIndicator.Hide();
    }





    //local network stuff
    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    void ShootRpc(Vector2 direction, float force)
    {
        StartCoroutine(Shoot(direction, force));
    }




}



//using UnityEngine;
//using System.Collections;

//public class ProjectileSpawner : MonoBehaviour
//{
//    public Transform FirePoint;
//    public GameObject ProjectilePrefab;
//    public Animator Animator;
//    public TurnManager turnManager;

//    public float minForce = 5f;
//    public float maxForce = 25f;
//    private float forceChanging = 5f;

//    private float currForce;
//    private bool forceIncreasing = true;
//    public bool canShoot = true;

//    void Start()
//    {
//        currForce = minForce;
//    }

//    public ForceIndicator forceIndicator;

//    private void Update()
//    {
//        if (!canShoot) return;

//        if (Input.GetButton("Fire1"))
//        {
//            if (forceIncreasing)
//            {
//                currForce += forceChanging * Time.deltaTime;
//                if (currForce >= maxForce) { currForce = maxForce; forceIncreasing = false; }
//            }
//            else
//            {
//                currForce -= forceChanging * Time.deltaTime;
//                if (currForce <= minForce) { currForce = minForce; forceIncreasing = true; }
//            }

//            // update indicator while charging
//            if (forceIndicator != null)
//                forceIndicator.SetForce(currForce, minForce, maxForce);
//        }

//        if (Input.GetButtonUp("Fire1") && canShoot)
//        {
//            StartCoroutine(Shoot(FirePoint.right, currForce));
//            if (forceIndicator != null)
//                forceIndicator.Hide();
//        }
//    }

//    private void Update()
//    {
//        Debug.Log($"[{gameObject.name}] canShoot: {canShoot}");
//        if (!canShoot) return;

//        if (Input.GetButton("Fire1"))
//        {
//            if (forceIncreasing)
//            {
//                currForce += forceChanging * Time.deltaTime;
//                if (currForce >= maxForce)
//                {
//                    currForce = maxForce;
//                    forceIncreasing = false;
//                }
//            }
//            else
//            {
//                currForce -= forceChanging * Time.deltaTime;
//                if (currForce <= minForce)
//                {
//                    currForce = minForce;
//                    forceIncreasing = true;
//                }
//            }
//        }

//        if (Input.GetButtonUp("Fire1") && canShoot)
//        {
//            StartCoroutine(Shoot(FirePoint.right, currForce));  //changed
//        }
//    }

//    private IEnumerator Shoot()
//    {
//        canShoot = false;
//        Animator.SetBool("isThrowing", true);

//        yield return new WaitForSeconds(0.7f); // wait for the throwing animation to play

//        GameObject projectile = Instantiate(ProjectilePrefab, FirePoint.position, FirePoint.rotation);
//        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
//        if (rb != null)
//        {
//            rb.AddForce(FirePoint.right * currForce, ForceMode2D.Impulse);
//        }

//        Animator.SetBool("isThrowing", false);

//        currForce = minForce;
//        forceIncreasing = true;

//        if (turnManager != null)
//        {
//            turnManager.NextTurn();
//        }

//    }



//    public void ComputerShoot(Vector2 direction, float force)
//    {
//        if (!canShoot) return;
//        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
//        FirePoint.rotation = Quaternion.Euler(0f, 0f, angle);
//        Debug.Log($"[Computer] Shooting angle: {angle}, force: {force}");
//        StartCoroutine(Shoot(FirePoint.right, force));
//    }
//}