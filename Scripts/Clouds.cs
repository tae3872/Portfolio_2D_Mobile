using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    public float speed;
    public int nextVec;

    SpriteRenderer spriteRenderer;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        Think();
        spriteRenderer.flipX = nextVec == -1 ? true : false;
    }
    void Update()
    {
        transform.Translate(new Vector2(nextVec, 0) * speed * Time.deltaTime);
        Vector2 rayVec = new Vector2(transform.position.x + nextVec * 1.5f, transform.position.y);
        Debug.DrawRay(rayVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rayVec, Vector3.down, 1, LayerMask.GetMask("CloudWall"));
        if (rayHit.collider != null)
        {
            nextVec *= -1;
        }
    }
    void Think()
    {
        nextVec = Random.Range(-1, 2);
        if (nextVec == 0)
        {
            Think();
        }
    }
}
