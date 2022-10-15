using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxManager : MonoBehaviour
{
    public Material[] bgMaterials;
    public GameObject player;


    private int bgIndex;
    public int height;

    void OnEnable()
    {
        RenderSettings.skybox = bgMaterials[player.GetComponent<PlayerMove>().Level % 7];
    }
    
}
