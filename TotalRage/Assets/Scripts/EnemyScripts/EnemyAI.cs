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
    public float EnemyDestinationRange;

    public float EnemyInteractionRange;
    private bool _playerWithinInteractionRange;

    public float EnemyAttackRange, EnemyAttackTime;
    private bool _playerWithinAttackRange, _enemyCanAttack = true;
    public Transform EnemyFirePosition;

    public bool MeleeEnemy;
    Animator EnemyMeleeAnimator;
    Animator EnemyRangeAnimator;
    Animator EnemyMeleeZombieAnimator;
    public int MeleeDamageValue = 2;

    void Start()
    {
        EnemyMeleeZombieAnimator = GetComponent<Animator>();
        EnemyRangeAnimator = GetComponent<Animator>();
        EnemyMeleeAnimator = GetComponent<Animator>();
        Player = FindObjectOfType<Player>().transform;
        EnemyNavMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void FixedUpdate()
    {
        ChasingPlayer();

        _playerWithinInteractionRange = Physics.CheckSphere(transform.position, EnemyInteractionRange, WhatIsPlayer);
        _playerWithinAttackRange = Physics.CheckSphere(transform.position, EnemyAttackRange, WhatIsPlayer);
        if (_playerWithinInteractionRange && _playerWithinAttackRange)
        {
            AttackingPlayer();
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
            AudioManager.Instance.PlaySound("RobotShot");
            StartCoroutine(ResetEnemyAttack());
        }

        else if (_enemyCanAttack && MeleeEnemy)
        {
            _enemyCanAttack = false;
            EnemyMeleeAnimator.SetTrigger("Attack");
            EnemyMeleeZombieAnimator.SetTrigger("Attack");
            StartCoroutine(ResetEnemyAttack());
        }
    }
    public void MeleeDamage()
    {
        // If player is within enemy attack range when enemy animation hits a certain point, player will take damage
        if(_playerWithinAttackRange)
        {
            Player.GetComponent<PlayerHealthSystem>().PlayerTakeDamage(MeleeDamageValue);
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
    private void PlayRobotBigStep()
    {
        if (_playerWithinInteractionRange)
        {
            AudioManager.Instance.PlaySound("RobotBigStep");
        }
    }
    private void PlayRobotSmallStep()
    {
        if (_playerWithinInteractionRange)
        {
            AudioManager.Instance.PlaySound("RobotSmallStep");
        }
    }
    private void PlayMeleeEnemyGrowl()
    {
        AudioManager.Instance.PlaySound("MeleeEnemyGrowl");
    }
    private void PlayMonsterFootstep()
    {
        if (_playerWithinInteractionRange)
        {
            AudioManager.Instance.PlaySound("MonsterFootstep");
        }
    }
}
