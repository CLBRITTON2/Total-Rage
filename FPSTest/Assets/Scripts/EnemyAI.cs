using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    NavMeshAgent enemyNavMeshAgent;
    public LayerMask whatIsGround;

    public Vector3 enemyDestinationPoint;
    bool destinationSet;
    public float enemyDestinationRange;

    // Start is called before the first frame update
    void Start()
    {
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Guarding();
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
}
