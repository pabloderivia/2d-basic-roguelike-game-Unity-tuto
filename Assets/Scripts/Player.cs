using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingbObject
{

    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;

    private Animator animator;

    private int food;
    
    // Start is called before the first frame update
    protected override void Start()
    {   
        animator = GetComponent<Animator>();
        food = GameManager.sharedInstance.playerFoodPoints;

        base.Start();
        
    }
    //An Unity's API function. It's called when the GO is disabled. We will do it to update the score when we change the levels.
    private void OnDisable() {
        GameManager.sharedInstance.playerFoodPoints = food;
    }

    private void CheckIfGameOver()
    {
        if(food<=0)
        {
            GameManager.sharedInstance.GameOver();
        }
    }
    //T variable: the component that we hope to encouter with
    protected override void AttemptMove <T>(int xDir, int yDir)
    {
        food--;
        base.AttemptMove <T> (xDir, yDir);

        RaycastHit2D hit;

        CheckIfGameOver();

        //If Move returns true, meaning Player was able to move into an empty space.
        if (Move (xDir, yDir, out hit)) 
           {
           }

           //Since the player has moved and lost food points, check if the game has ended.
            CheckIfGameOver ();

            //Set the playersTurn boolean of GameManager to false now that players turn is over.
            GameManager.sharedInstance.playersTurn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.sharedInstance.playersTurn) return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int) Input.GetAxisRaw("Horizontal");
        vertical = (int) Input.GetAxisRaw("Vertical");
        if (horizontal!=0)
            vertical = 0;
        
        if(horizontal !=0 || vertical!=0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }

        
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("PlayerChop");
    }

    private void Restart()
    {
        SceneManager.LoadScene("MainScene");   
    }

    public void LoseFood (int loss)
    {
        animator.SetTrigger("PlayerHit");
        food -= loss;
        CheckIfGameOver();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag== "Exit")
        {
            //invoke a function with a certain delay (1 sec)
            Invoke("Restart",restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            other.gameObject.SetActive(false);
        }
        else if (other.tag=="Soda")
        {
            food += pointsPerSoda;
            other.gameObject.SetActive(false);
        }
    }
}
