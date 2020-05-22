using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MoovingObject
{
    public int wallDamage = 1;
    public float restartLevelDelay = 1f;
    public Text foodText;
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip gameOverSound;

    private Animator animator;
    private int food;
    private List<AudioClip> ObjectSound;

    // Start is called before the first frame update
    protected override void Start()
    {
        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoints;

        foodText.text = "HP: " + food;

        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.playersTurn == false)
        {
            return;
        }

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        foodText.text = "HP: " + food;

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        if (Move(xDir,yDir,out hit))
        {
            SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
        }

        CheckIfGameOver();

        GameManager.instance.playersTurn = false;
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Soda" || other.tag == "Food")
        {
            ObjectSound = new List<AudioClip>(GetObjectSounds(other));
            int value = other.gameObject.GetComponent<IReturnValue>().ValuePoints;

            food += value;
            foodText.text = "+" + value + " HP: " + food;
            SoundManager.instance.RandomizeSfx(ObjectSound[0], ObjectSound[1]);
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Enemy")
        {
            LoseFood(other.gameObject.GetComponent<Enemy>().PlayerDamage);
            SoundManager.instance.RandomizeSfx(other.gameObject.GetComponent<Enemy>().EnemyAttak1, other.gameObject.GetComponent<Enemy>().EnemyAttak2);
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnCantMove<T> (T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
    }

    private List<AudioClip> GetObjectSounds(Collider2D other)
    {
        List<AudioClip> audio = new List<AudioClip>()
        {
            other.gameObject.GetComponent<IReturnValue>().Sound1,
            other.gameObject.GetComponent<IReturnValue>().Sound2
        };

        return audio;
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        foodText.text = "-" + loss + " HP: " + food;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            GameManager.instance.GameOver();
        }
    }
}
