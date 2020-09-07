using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public BoardManager boardScript;
    public static GameManager sharedInstance;
    private int level = 4;

    private void Awake() 
    {
        if (sharedInstance==null)
            sharedInstance = this;
        
        else if (sharedInstance!=this)
            Destroy(gameObject);
        
        //When we load a new Scene, this script will perdure
        DontDestroyOnLoad(gameObject);    
        boardScript = GetComponent<BoardManager>();
        InitGame();    
    }

    void InitGame()
    {
        boardScript.SetupScene(level);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
