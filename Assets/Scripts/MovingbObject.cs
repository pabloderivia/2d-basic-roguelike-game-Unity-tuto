using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingbObject : MonoBehaviour
{
    public float moveTime = 1.5f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rigidbody;
    //we will use it for multiply, instead of divide, which is more efficiently
    private float inverseMoveTime;
    //PRUEBA
    bool isMoving;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f/moveTime;
        isMoving = false;

    }

    protected IEnumerator SmoothMovement (Vector3 end)
    {
        isMoving = true;
        //the square magnitud is more efficient than magnitud
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon) //Epsion = the smallest number a float can get >0
        {
            Vector3 newPos = Vector3.MoveTowards(rigidbody.position, end, inverseMoveTime * Time.deltaTime);
            rigidbody.MovePosition(newPos);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            // we will wait for a frame before reevaluating the condition of the loop
            yield return null;
        }
        isMoving = false;
    }
    //"Out" sirve para que el método actualice una de las variables que se le introduce como parámetro
    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {

        Vector2 start = transform.position;
        Vector2 end = start + new Vector2 (xDir, yDir);
        //we will desable boxCollider to be sure the linecast doesnt find it
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        //PRUEBA
        if (isMoving)
            return false;
        //successful movement
        if (hit.transform == null)
        {
            StartCoroutine(SmoothMovement(end));
            return true;
        }

        //fail
        else 
            return false;
    }
    //"T" will be the component who is in the position which  the moving object tried to occupe
    //For the enemies, T will be the player. For the player, the breakable walls.
    protected virtual void AttemptMove <T> (int xDir, int yDir)
    where T: Component
    {
        RaycastHit2D hit;
        //"hit" value is updated by Move();
        bool canMove = Move (xDir, yDir, out hit);
        
        if (hit.transform == null)
            return;
        
        T hitComponent = hit.transform.GetComponent<T>();
        //si no se puede mover, y ha tocado algo: 
        if(!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
        
    }
    protected abstract void OnCantMove <T> (T component)
        where T: Component;


}
