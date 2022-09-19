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

    public float enemyAttackRange, enemyAttackTime;
    private bool playerWithinAttackRange, enemyCanAttack = true;
    public Transform enemyFirePosition;

    public bool meleeEnemy;
    Animator enemyMeleeAnimator;
    Animator enemyRangeAnimator;

    // Start is called before the first frame update
    void Start()
    {
        enemyRangeAnimator = GetComponent<Animator>();
        enemyMeleeAnimator = GetComponent<Animator>();
        player = FindObjectOfType<Player>().transform;
        enemyNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        playerWithinInteractionRange = Physics.CheckSphere(transform.position, enemyInteractionRange, whatIsPlayer);
        playerWithinAttackRange = Physics.CheckSphere(transform.position, enemyAttackRange, whatIsPlayer);

        if (!playerWithinInteractionRange && !playerWithinAttackRange)
        {
            Guarding();
        }
        if (playerWithinInteractionRange && !playerWithinAttackRange)
        {
            ChasingPlayer();
        }
        if (playerWithinInteractionRange && playerWithinAttackRange)
        {
            AttackingPlayer();
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
    private void AttackingPlayer()
    {
        enemyNavMeshAgent.SetDestination(transform.position);
        transform.LookAt(player);

        if (enemyCanAttack && !meleeEnemy)
        {
            enemyCanAttack = false;
            enemyFirePosition.LookAt(player);
            enemyRangeAnimator.SetTrigger("Attack");
            ObjectPoolManager.instance.SpawnFromObjectPool("Enemy Projectile", enemyFirePosition.position, enemyFirePosition.rotation);
            StartCoroutine(ResetEnemyAttack());
        }
        else if (enemyCanAttack && meleeEnemy)
        {
            enemyMeleeAnimator.SetTrigger("Attack");
        }
    }
    public void MeleeDamage()
    {
        if(playerWithinAttackRange)
        {
            // TODO add damage to player
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
    IEnumerator ResetEnemyAttack()
    {
        yield return new WaitForSeconds(enemyAttackTime);

        enemyCanAttack = true;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, enemyInteractionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyAttackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, enemyDestinationRange);
    }
}
