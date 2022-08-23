using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicMovement : MonoBehaviour
{
    Animator enemyAnimator;
    Transform player;

    public bool move, rotate;

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        player = FindObjectOfType<Player>().transform;
        enemyAnimator.SetBool("Move", move);
        enemyAnimator.SetBool("Rotating", rotate);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
    }
}
