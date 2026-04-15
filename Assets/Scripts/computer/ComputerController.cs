using UnityEngine;
using System.Collections;

public class ComputerController : MonoBehaviour
{
    [Header("Settings")]
    [Range(0f, 1f)]
    public float accuracy = 0.3f;  //0.8-->hard, 0.5--> mid, 0.3-->easy
    public float reactionDelay = 1.5f;
    public int playerNumber = 2;

    private ProjectileSpawner spawner;
    private Transform target;

    public void Init()
    {
        spawner = GetComponent<ProjectileSpawner>();
        target = playerNumber == 1
            ? GameManager.Instance.GetPlayer2Transform()
            : GameManager.Instance.GetPlayer1Transform();

        Debug.Log($"[Computer] playerNumber: {playerNumber}");
        Debug.Log($"[Computer] humanPlaysAsPlayer1: {GameSettings.humanPlaysAsPlayer1}");
        Debug.Log($"[Computer] target: {target.name}");
        Debug.Log($"[Computer] accuracy set to: {accuracy}");

        StartCoroutine(ComputerLoop());
    }

    IEnumerator ComputerLoop()
    {
        while (true)
        {
            Debug.Log($"[Computer] Waiting... currPlayer: {TurnManager.Instance.currPlayer}, myNumber: {playerNumber}");
            yield return new WaitUntil(() =>
                TurnManager.Instance.currPlayer.Value == playerNumber);

            Debug.Log("[Computer] My turn! Shooting...");
            yield return new WaitForSeconds(reactionDelay);
            spawner.ComputerShoot(GetAimedDirection(), GetAimedForce());
        }
    }


    float GetAimedForce()
    {
        float distance = Vector2.Distance(transform.position, target.position);

        // scale force
        float normalizedDist = Mathf.Clamp01(distance / 50f);
        float exactForce = Mathf.Lerp(spawner.minForce, spawner.maxForce, normalizedDist);

        // apply accuracy error to force too
        float forceError = Random.Range(-3f, 3f) * (1f - accuracy);
        return Mathf.Clamp(exactForce + forceError, spawner.minForce, spawner.maxForce);
    }

    private Vector2 lastAimedDirection;

    Vector2 GetAimedDirection()
    {
        Vector2 myPos = transform.position;
        Vector2 targetPos = target.position;

        Vector2 exactDir = (targetPos - myPos).normalized;
        exactDir.y += 0.8f;  // control the angle
        exactDir.Normalize();

        float error = Random.Range(-1f, 1f) * (1f - accuracy);
        exactDir.x += error;
        exactDir.y += error * 0.3f;

        lastAimedDirection = exactDir.normalized;
        return lastAimedDirection;
    }


    // testing
    void OnDrawGizmos()
    {
        if (target == null || spawner == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, target.position);

        // use cached direction, not recalculated
        if (lastAimedDirection != Vector2.zero)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + lastAimedDirection * 5f);
        }
    }
}