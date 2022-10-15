using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Camposition : MonoBehaviour
{
    public Transform player;
    Vector3 offset;

    private void Start()
    {
        offset = transform.position - player.position;
    }
    void Update()
    {

        if (player.transform.position.y <= -3f)
        {
            return;
        }
        this.transform.position = new Vector3
            (this.transform.position.x, player.transform.position.y + offset.y, this.transform.position.z);
    }

}
