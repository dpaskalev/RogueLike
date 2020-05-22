using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int playerDamage;

    public int PlayerDamage
    {
        get { return playerDamage; }
        private set { playerDamage = value; }
    }

    [SerializeField]
    private AudioClip enemyAttak1;

    public AudioClip EnemyAttak1
    {
        get { return enemyAttak1; }
        private set { enemyAttak1 = value; }
    }

    [SerializeField]
    private AudioClip enemyAttak2;

    public AudioClip EnemyAttak2
    {
        get { return enemyAttak2; }
        private set { enemyAttak2 = value; }
    }

    private Animator animator;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        GameManager.GetInstance().AddEnemyToList(this);
        animator = GetComponent<Animator>();

        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
    }


}
