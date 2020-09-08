using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public BoardManager boardScript;
    public static GameManager sharedInstance;
    private int level = 4;
    public int playerFoodPoints = 100;
    public bool playersTurn;

    //delay for turn change
    public float turnDelay = .1f;
    //list or current enemies, to control their moves
    private List<Enemy> enemies;
    private bool enemiesMoving;

    private void Awake() 
    {
        playersTurn = true;

        if (sharedInstance==null)
            sharedInstance = this;
        
        else if (sharedInstance!=this)
            Destroy(gameObject);
        
        //When we load a new Scene, this script will perdure
        DontDestroyOnLoad(gameObject); 
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        InitGame();    
    }

    void InitGame()
    {
        //the gamemanager will not be reseted when a new lv charged, so, we must clear our previous lv's enemies
        enemies.Clear();
        boardScript.SetupScene(level);
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playersTurn || enemiesMoving)
            return;

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if(enemies.Count == 0)
            yield return new WaitForSeconds(turnDelay);
        
        for (int i = 0; i<enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }

}
