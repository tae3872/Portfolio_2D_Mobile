using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    public int nextMove;
    public static int plusHeight;
    public PlayerMove player;
    // Start is called before the first frame update
    void Awake()
    {
        transform.position = new Vector2(0, 1000);
    }
    void OnEnable()
    {
        transform.position = new Vector2(0, 1000 + 1000 * (player.levelCycle));
        if (player.Level == 0)
            return;
        plusHeight += 1000 + (player.Level % 7 == 0 ? 1000 * (player.levelCycle - 1) : 1000 * (player.levelCycle));
    }
    void Start()
    {
        Think();
    }

    // Update is called once per frame
    void Update()
    {
        GroundPatrol();
    }
    void GroundPatrol()
    {
        transform.Translate(new Vector2(nextMove, 0f) * 0.03f);
        Vector2 frontVec = new Vector2(transform.position.x + nextMove, transform.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Wall"));
        if (rayHit.collider != null)
        {
            nextMove *= -1;
            CancelInvoke();
            float nextThinkTime = Random.Range(1, 2);
            Invoke("Think", nextThinkTime);
        }
    }
    void Think()
    {
        float nextThinkTime = Random.Range(1, 2);

        nextMove = Random.Range(-1, 2);


        Invoke("Think", nextThinkTime);
    }

}
