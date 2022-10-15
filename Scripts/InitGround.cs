using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitGround : MonoBehaviour
{
    public GameObject player;

    public int nextMove;
    public GameObject leftWall;
    public GameObject rightWall;

    void OnEnable()
    {
        nextMove = 0;
        Invoke("Think", 5f);
    }
    void OnDisable()
    {
        CancelInvoke();
    }
    private void Update()
    {
        GroundPatrol();
    }

    void GroundPatrol()
    {
        transform.Translate(new Vector2(nextMove, 0f) * 0.01f);
        Vector2 frontVec = new Vector2(transform.position.x + nextMove * gameObject.transform.localScale.x * 0.15f, transform.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Wall"));
        if (rayHit.collider != null)
        {
            nextMove *= -1;
            //CancelInvoke();
            //float nextThinkTime = Random.Range(2, 4);
            //Invoke("Think", nextThinkTime);
            //player.GetComponent<Rigidbody2D>().velocity= new Vector2(0, player.GetComponent<Rigidbody2D>().velocity.y);
        }
    }
    void Think()
    {
        float nextThinkTime = Random.Range(7, 10);

        nextMove = Random.Range(-1, 2);
        Invoke("Think", nextThinkTime);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !player.GetComponent<PlayerMove>().isWallTouch)
        {
            collision.transform.parent = gameObject.transform;
        }
        else if (player.GetComponent<PlayerMove>().isWallTouch)
        {
            collision.transform.parent = null;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerMove playerLogic = collision.gameObject.GetComponent<PlayerMove>();
            playerLogic.rigid.velocity = new Vector2(playerLogic.rigid.velocity.x, 0);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.parent = null;

        }
    }

}
