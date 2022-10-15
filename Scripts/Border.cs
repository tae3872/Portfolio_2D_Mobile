using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    public PlayerMove player;
    void Start()
    {
        //transform.position = new Vector2(0, 1000);
    }
    void OnEnable()
    {
        transform.position = new Vector2(0, 1005 + 1000 * (player.levelCycle));

    }    
}
