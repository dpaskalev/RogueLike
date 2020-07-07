using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    private int columns;

    public int Columns
    {
        get { return columns; }
        private set { columns = value; }
    }

    [SerializeField]
    private int rows;

    public int Rows
    {
        get { return rows; }
        private set { rows = value; }
    }

    //[SerializeField]
    //int foodMin = 1;

    [SerializeField]
    int foodMax = 10;

    [SerializeField]
    private GameObject exit;

    public GameObject Exit
    {
        get { return exit; }
        private set { exit = value; }
    }

    [SerializeField]
    private List<GameObject> floorTiles = new List<GameObject>();
    [SerializeField]
    private List<GameObject> foodTiles = new List<GameObject>();
    [SerializeField]
    private List<GameObject> enemyTiles = new List<GameObject>();
    [SerializeField]
    private List<GameObject> outerWallTiles = new List<GameObject>();

    private List<Vector3> foodLocations = new List<Vector3>();
    
    private Transform boardHolder;

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Count)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Count)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    void LayoutEnemies(List<GameObject> enemyTiles, List<Vector3> foodLocations)
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if ((x == 0 && y == 0) || (x == columns - 1 && y == rows - 1) || GetIfFoodLocation(x, y, foodLocations))
                {
                    continue;
                }
                else
                {
                    Instantiate(enemyTiles[Random.Range(0, enemyTiles.Count)], new Vector3(x, y, 0f), Quaternion.identity);
                }
            }
        }
    }

    void LayoutFoodAtRandom(List<GameObject> foodTiles, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 vec = new Vector3(Random.Range(1, columns - 1), Random.Range(1, rows - 1), 0f);

            while (true)
            {
                vec = new Vector3(Random.Range(1, columns - 1), Random.Range(1, rows - 1), 0f);

                if (GetIfFoodLocation(vec.x, vec.y, foodLocations) == false)
                {
                    break;
                }
            }
            
            
            Instantiate(foodTiles[Random.Range(0, foodTiles.Count)], vec, Quaternion.identity);
            foodLocations.Add(vec);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        LayoutFoodAtRandom(foodTiles, Random.Range(1, foodMax));
        LayoutEnemies(enemyTiles, foodLocations);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }

    private bool GetIfFoodLocation(float x, float y, List<Vector3> foodLocations)
    {
        bool result = false;

        foreach (Vector3 location in foodLocations)
        {
            if (location.x == x && location.y == y)
            {
                result = true;
                break;
            }
        }

        return result;
    }
}
