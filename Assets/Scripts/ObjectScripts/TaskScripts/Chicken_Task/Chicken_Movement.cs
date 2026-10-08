using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Chicken_Movement : MonoBehaviour
{
    private enum State { Roaming, Fleeing }

    [Header("Movimiento")]
    [SerializeField] private float roamRadius = 8f;
    [SerializeField] private float wanderInterval = 3f;
    [SerializeField] private float baseSpeed = 2f;

    [Header("Detección / Huida")]
    [SerializeField] private Transform playerTransform; // si se deja vacío, busca por tag "Player"
    [SerializeField] private float detectionRadius = 4f;
    [SerializeField] private float fleeDistance = 6f;
    [SerializeField] private float fleeSpeedMultiplier = 1.5f;
    [SerializeField] private float stopFleeMultiplier = 1.25f; // cuando el jugador esté a > detectionRadius * este valor, deja de huir

    private NavMeshAgent agent;
    private State currentState = State.Roaming;
    private Coroutine wanderCoroutine;
    private float currentBaseSpeed;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("Chicken_Movement necesita un NavMeshAgent en el mismo GameObject.");
            enabled = false;
            return;
        }

        currentBaseSpeed = baseSpeed;
        agent.speed = baseSpeed;

        if (playerTransform == null)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go != null) playerTransform = go.transform;
        }
    }

    private void OnEnable()
    {
        StartRoaming();
    }

    private void OnDisable()
    {
        if (wanderCoroutine != null) StopCoroutine(wanderCoroutine);
    }

    private void Update()
    {
        if (playerTransform == null) return;

        float distToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (currentState == State.Roaming)
        {
            if (distToPlayer <= detectionRadius)
            {
                StartFleeing();
            }
        }
        else if (currentState == State.Fleeing)
        {
            // Si el jugador está suficientemente lejos, volver a deambular
            if (distToPlayer > detectionRadius * stopFleeMultiplier)
            {
                StartRoaming();
            }
            else
            {
                // Si el agente ha llegado cerca del destino y aún detecta al jugador, recalcula para alejarse más
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
                {
                    SetFleeDestination();
                }
            }
        }
    }

    private void StartRoaming()
    {
        currentState = State.Roaming;
        agent.speed = currentBaseSpeed;
        if (wanderCoroutine != null) StopCoroutine(wanderCoroutine);
        wanderCoroutine = StartCoroutine(WanderRoutine());
    }

    private void StartFleeing()
    {
        currentState = State.Fleeing;
        if (wanderCoroutine != null) StopCoroutine(wanderCoroutine);
        agent.speed = currentBaseSpeed * fleeSpeedMultiplier;
        SetFleeDestination();
    }

    private IEnumerator WanderRoutine()
    {
        while (true)
        {
            Vector3 randomPoint;
            if (TryGetRandomPoint(transform.position, roamRadius, out randomPoint))
            {
                agent.SetDestination(randomPoint);
            }
            yield return new WaitForSeconds(wanderInterval);
        }
    }

    private void SetFleeDestination()
    {
        if (playerTransform == null) return;

        Vector3 awayDir = (transform.position - playerTransform.position).normalized;
        Vector3 desiredPos = transform.position + awayDir * fleeDistance;

        Vector3 navPos;
        if (TryGetRandomPointNear(desiredPos, 1.5f, out navPos))
        {
            agent.SetDestination(navPos);
        }
        else
        {
            // fallback: intenta un punto aleatorio en un radio pequeño
            Vector3 fallback;
            if (TryGetRandomPoint(transform.position, roamRadius, out fallback))
            {
                agent.SetDestination(fallback);
            }
        }
    }

    // Busca un punto aleatorio en NavMesh dentro de un radio alrededor de origin
    private bool TryGetRandomPoint(Vector3 origin, float radius, out Vector3 result)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += origin;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, 2f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = origin;
        return false;
    }

    // Intenta muestrear cerca de una posición deseada
    private bool TryGetRandomPointNear(Vector3 desired, float searchRadius, out Vector3 result)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(desired, out hit, searchRadius, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = desired;
        return false;
    }

    // Visualización en editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, roamRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fleeDistance);
    }
}
