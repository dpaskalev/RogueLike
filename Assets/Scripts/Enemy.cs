using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int playerDamage;

    private Animator animator;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;

    //private bool skipMove;
    public AudioClip enemyAttak1;
    public AudioClip enemyAttak2;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();

        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }
}
