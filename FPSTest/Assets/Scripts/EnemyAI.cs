using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    NavMeshAgent enemyNavMeshAgent;
    public LayerMask whatIsGround, whatIsPlayer;
    public Transform player;

    public Vector3 enemyDestinationPoint;
    bool destinationSet;
    public float enemyDestinationRange;

    public float enemyInteractionRange;
    private bool playerWithinInteractionRange;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().transform;
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        playerWithinInteractionRange = Physics.CheckSphere(transform.position, enemyInteractionRange, whatIsPlayer);

        if (!playerWithinInteractionRange)
        {
            Guarding();
        }
        else if (playerWithinInteractionRange)
        {
            ChasingPlayer();
        }
    }
    private void Guarding()
    {
        if(!destinationSet)
        {
            SearchForDestination();
        }
        else
        {
            enemyNavMeshAgent.SetDestination(enemyDestinationPoint);
        }

        Vector3 distanceToDestination = transform.position - enemyDestinationPoint;
        if(distanceToDestination.magnitude < 1f)
        {
            destinationSet = false;
        }
    }
    private void ChasingPlayer()
    {
        enemyNavMeshAgent.SetDestination(player.position);
    }
    private void SearchForDestination()
    {
        // Create random point for enemy to walk to
        float randomEnemyPositionZ = Random.Range(-enemyDestinationRange, enemyDestinationRange);
        float randomEnemyPositionX = Random.Range(-enemyDestinationRange, enemyDestinationRange);

        enemyDestinationPoint = new Vector3(
            transform.position.x + randomEnemyPositionX,
            transform.position.y,
            transform.position.z + randomEnemyPositionZ);

        if(Physics.Raycast(enemyDestinationPoint, -transform.up, 2f, whatIsGround))
        {
            destinationSet = true;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyInteractionRange);
    }
}
