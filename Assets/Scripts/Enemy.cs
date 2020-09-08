using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingbObject
{
    public int playerDamage;

    private Animator animator;
    private Transform target;
    private bool skipMove;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    protected override void Start()
    {
        GameManager.sharedInstance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        base.Start();
        
    }


    protected override void AttemptMove <T> (int xDir, int yDir)
    {
        if (skipMove)
        {
            skipMove = false;
            return;
        }

        base.AttemptMove<T> (xDir, yDir);
        skipMove = true;
    }

    public void MoveEnemy()
    {
        int xDir = 0; 
        int yDir =0;
        //if we are at the same column, we will move vertically. If we are not, we will approach horizontally to the player
        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            yDir = target.position.y > transform.position.y ? 1: -1;
        
        else 
            if(target.position.x > transform.position.x)
            {
               xDir = 1;
               spriteRenderer.flipX = true;
            }
            else
            {
               spriteRenderer.flipX = false;
               xDir = -1;
            }

        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;
        animator.SetTrigger("EnemyAttack");
        hitPlayer.LoseFood(playerDamage);
    }

    void LookDirection()
    {}
}
