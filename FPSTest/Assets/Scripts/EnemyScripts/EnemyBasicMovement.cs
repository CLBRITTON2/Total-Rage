using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicMovement : MonoBehaviour
{
    Animator EnemyAnimator;
    Transform Player;

    public bool move, rotate;

    // Start is called before the first frame update
    void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
        Player = FindObjectOfType<Player>().transform;
        EnemyAnimator.SetBool("Move", move);
        EnemyAnimator.SetBool("Rotating", rotate);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Player);
    }
}
