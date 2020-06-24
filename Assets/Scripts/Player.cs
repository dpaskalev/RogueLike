using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MoovingObject
{
    [SerializeField]
    private int wallDamage;

    public int WallDamage
    {
        get { return wallDamage; }
        private set { wallDamage = value; }
    }

    [SerializeField]
    private float restartLevelDelay;

    public float RestartLevelDelay
    {
        get { return restartLevelDelay; }
        private set { restartLevelDelay = value; }
    }

    [SerializeField]
    private Text foodText;

    public Text FoodText
    {
        get { return foodText; }
        private set { foodText = value; }
    }

    [SerializeField]
    private AudioClip moveSound1;

    public AudioClip MoveSound1
    {
        get { return moveSound1; }
        private set { moveSound1 = value; }
    }

    [SerializeField]
    private AudioClip moveSound2;

    public AudioClip MoveSound2
    {
        get { return moveSound2; }
        private set { moveSound2 = value; }
    }

    [SerializeField]
    private AudioClip gameOverSound;

    public AudioClip GameOverSound
    {
        get { return gameOverSound; }
        private set { gameOverSound = value; }
    }

    private Animator animator;
    private int food;
    private List<AudioClip> ObjectSound;

    protected override void Start()
    {
        animator = GetComponent<Animator>();

        food = GameManager.GetInstance().PlayerFoodPoints;

        foodText.text = "HP: " + food;

        base.Start();
    }

    private void OnDisable()
    {
        GameManager.GetInstance().PlayerFoodPoints = food;
    }

    void Update()
    {
        if (GameManager.GetInstance().PlayersTurn == false)
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
            SoundManager.GetInstance().RandomizeSfx(moveSound1, moveSound2);
        }

        CheckIfGameOver();

        GameManager.GetInstance().PlayersTurn = false;
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            ObjectSound = new List<AudioClip>(GetObjectSounds(other));
            int value = other.gameObject.GetComponent<IReturnValue>().ValuePoints;

            food += value;
            foodText.text = "+" + value + " HP: " + food;
            SoundManager.GetInstance().RandomizeSfx(ObjectSound[0], ObjectSound[1]);
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Enemy")
        {
            LoseFood(other.gameObject.GetComponent<Enemy>().PlayerDamage);
            SoundManager.GetInstance().RandomizeSfx(other.gameObject.GetComponent<Enemy>().EnemyAttak1, other.gameObject.GetComponent<Enemy>().EnemyAttak2);
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
            SoundManager.GetInstance().PlaySingle(gameOverSound);
            SoundManager.GetInstance().musicSource.Stop();
            GameManager.GetInstance().GameOver();
        }
    }
}
