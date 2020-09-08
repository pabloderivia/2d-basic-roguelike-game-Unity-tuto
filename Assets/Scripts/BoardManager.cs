using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    // we will use it to determinate a range to pick a random number of walls or food 
    public class Count
    {
        public int min; 
        public int max;

        public Count(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }
    //board dimentions
    public int cols = 8;
    public int rows = 8;
    
    public Count wallCount = new Count(5,9);
    public Count foodCount = new Count(1,5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;
    //just the parent to all these GO's
    private Transform boardHolder;
    //a register of the avalaible and busy positions of the grid
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList()
    {
        //we leave a free external square to avoid produce impassable levels
        gridPositions.Clear();
        for (int x = 1; x<cols -1; x++)
        {
            for (int y = 1; y<rows-1;y++)
            {
                gridPositions.Add(new Vector3(x,y,0f));
            }
        }
    }
    //it will paint the floor and walls 
    void BoardSetup()
    {
        boardHolder = new GameObject ("Board").transform;

        for (int x = -1; x<cols+1; x++)
        {
            for (int y = -1; y<rows+1; y++)
            {
                GameObject floorToInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                //if its on the limits of the board, we will use our wall tiles
                if(x==-1 || x==cols || y==-1 || y == rows)
                    floorToInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                GameObject newFloorPiece = Instantiate(floorToInstantiate, new Vector3 (x,y,0f), Quaternion.identity);

                newFloorPiece.transform.SetParent(boardHolder);

                
            }
        }
    }

    Vector3 RandomPosition ()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPos = gridPositions[randomIndex];
        //we delete this position for the list to avoid fill 2 times the same position
        gridPositions.RemoveAt(randomIndex);
        return randomPos;
    }
    //spawn the objects of a kind
    void GenerateTilesOfAKind(GameObject[] tileArray, int min, int max)
    {
        //control how many object of a same kind we will spawn, for ex, the number of walls in a lv
        int objectCount = Random.Range(min, max+1);

        for (int i = 0; i <objectCount; i++)
        {
            Vector3 randomPos = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate (tileChoice, randomPos, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();
        GenerateTilesOfAKind(wallTiles, wallCount.min, wallCount.max);
        GenerateTilesOfAKind(foodTiles, foodCount.min, foodCount.max);
        int enemyCount = (int) Mathf.Log(level, 2f);
        Debug.Log("se generarán "+enemyCount+" enemigos");
        GenerateTilesOfAKind(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(cols-1, rows-1, 0f), Quaternion.identity);

    }

}
