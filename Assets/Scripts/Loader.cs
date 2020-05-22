using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    [SerializeField]
    private GameObject gameManager;

    public GameObject GameManagerProperty
    {
        get { return gameManager; }
        private set { gameManager = value; }
    }

    void Awake()
    {
        if (GameManager.GetInstance() == null)
        {
            Instantiate(gameManager);
        }
    }
}
