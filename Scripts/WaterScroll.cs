using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScroll : MonoBehaviour
{
    public bool leftDir;
    public float speed;
    public int startIndex;
    public int endIndex;

    public Transform[] sprites;
    public float scrollPoint;

    void Update()
    {
        Move();
        Scrolling();
    }
    void Move()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }
    void Scrolling()
    {
        if (sprites[endIndex].position.x >= scrollPoint && !leftDir)
        {
            Vector3 initPos = sprites[startIndex].localPosition;
            sprites[endIndex].transform.localPosition = initPos - (new Vector3(scrollPoint, 0, 0));
            int save = endIndex;
            startIndex = endIndex;
            endIndex = (save - 1 == -1) ? 2 : save - 1;
        }
        else if (sprites[startIndex].position.x <= -scrollPoint && leftDir)
        {
            Vector3 initPos = sprites[endIndex].localPosition;
            sprites[startIndex].transform.localPosition = initPos + (new Vector3(scrollPoint, 0, 0));
            int save = startIndex;
            endIndex = startIndex;
            startIndex = (save - 1 == -1) ? 2 : save - 1;
        }
    }
}
