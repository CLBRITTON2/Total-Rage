using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    NavMeshAgent EnemyNavMeshAgent;
    public LayerMask WhatIsGround, WhatIsPlayer;
    public Transform Player;

    public Vector3 EnemyDestinationPoint;
    bool DestinationSet;
    public float EnemyDestinationRange;

    public float EnemyInteractionRange;
    private bool _playerWithinInteractionRange;

    public float EnemyAttackRange, EnemyAttackTime;
    private bool _playerWithinAttackRange, _enemyCanAttack = true;
    public Transform EnemyFirePosition;

    public bool MeleeEnemy;
    Animator EnemyMeleeAnimator;
    Animator EnemyRangeAnimator;
    public int MeleeDamageValue = 2;

    // Start is called before the first frame update
    void Start()
    {
        EnemyRangeAnimator = GetComponent<Animator>();
        EnemyMeleeAnimator = GetComponent<Animator>();
        Player = FindObjectOfType<Player>().transform;
        EnemyNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        _playerWithinInteractionRange = Physics.CheckSphere(transform.position, EnemyInteractionRange, WhatIsPlayer);
        _playerWithinAttackRange = Physics.CheckSphere(transform.position, EnemyAttackRange, WhatIsPlayer);

        if (!_playerWithinInteractionRange && !_playerWithinAttackRange)
        {
            Guarding();
        }
        if (_playerWithinInteractionRange && !_playerWithinAttackRange)
        {
            ChasingPlayer();
        }
        if (_playerWithinInteractionRange && _playerWithinAttackRange)
        {
            AttackingPlayer();
        }
    }
    private void Guarding()
    {
        if(!DestinationSet)
        {
            SearchForDestination();
        }
        else
        {
            EnemyNavMeshAgent.SetDestination(EnemyDestinationPoint);
        }

        Vector3 distanceToDestination = transform.position - EnemyDestinationPoint;
        if(distanceToDestination.magnitude < 1f)
        {
            DestinationSet = false;
        }
    }
    private void ChasingPlayer()
    {
        EnemyNavMeshAgent.SetDestination(Player.position);
    }
    private void AttackingPlayer()
    {
        EnemyNavMeshAgent.SetDestination(transform.position);
        transform.LookAt(Player);

        if (_enemyCanAttack && !MeleeEnemy)
        {
            _enemyCanAttack = false;
            EnemyFirePosition.LookAt(Player);
            EnemyRangeAnimator.SetTrigger("Attack");
            ObjectPoolManager.Instance.SpawnFromObjectPool("Enemy Projectile", EnemyFirePosition.position, EnemyFirePosition.rotation);
            StartCoroutine(ResetEnemyAttack());
        }
        else if (_enemyCanAttack && MeleeEnemy)
        {
            EnemyMeleeAnimator.SetTrigger("Attack");
        }
    }
    public void MeleeDamage()
    {
        if(_playerWithinAttackRange)
        {
            Player.GetComponent<PlayerHealthSystem>().PlayerTakeDamage(MeleeDamageValue);
        }
    }
    private void SearchForDestination()
    {
        // Create random point for enemy to walk to
        float randomEnemyPositionZ = Random.Range(-EnemyDestinationRange, EnemyDestinationRange);
        float randomEnemyPositionX = Random.Range(-EnemyDestinationRange, EnemyDestinationRange);

        EnemyDestinationPoint = new Vector3(
            transform.position.x + randomEnemyPositionX,
            transform.position.y,
            transform.position.z + randomEnemyPositionZ);

        if(Physics.Raycast(EnemyDestinationPoint, -transform.up, 2f, WhatIsGround))
        {
            DestinationSet = true;
        }
    }
    IEnumerator ResetEnemyAttack()
    {
        yield return new WaitForSeconds(EnemyAttackTime);

        _enemyCanAttack = true;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, EnemyInteractionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, EnemyAttackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, EnemyDestinationRange);
    }
}
