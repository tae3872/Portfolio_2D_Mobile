using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public GameObject target;
    private void Update()
    {
        transform.position = new Vector2(transform.position.x, target.transform.position.y);
    }
}
