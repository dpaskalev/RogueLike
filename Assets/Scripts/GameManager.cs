using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public float levelStartDelay = 2f;
    public float turnDelay = .1f;
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector] public bool playersTurn = true;

    public List<Enemy> enemyes;
    private Text levelText;
    private GameObject levelImage;
    private int level = 1;
    private bool enemiesMoving;
    private bool doingSetup;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        enemyes = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    private void InitGame()
    {
        doingSetup = true;

        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);

        enemyes.Clear();
        boardScript.SetupScene(level);
    }

    private void OnLevelWasLoaded(int index)
    {
        level++;

        InitGame();
    }

    //void OnEnable()
    //{
    //    SceneManager.sceneLoaded += OnLevelFinishedLoading;
    //}

    //void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    //}

    //void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    //{
    //    Debug.Log("Level Loaded");
    //    Debug.Log(scene.name);
    //    Debug.Log(mode);
    //}

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "After " + level + " days, you starved";
        levelImage.SetActive(true);
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playersTurn || enemiesMoving || doingSetup)
        {
            return;
        }

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        enemyes.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;

        yield return new WaitForSeconds(2 * turnDelay);
        
        playersTurn = true;
        enemiesMoving = false;
    }
}
