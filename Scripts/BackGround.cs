using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGround : MonoBehaviour
{
    public GameObject[] clouds0;
    public GameObject[] clouds1;
    public GameObject[] clouds2;
    public GameObject[] clouds3;
    public GameObject[] clouds4;
    public GameObject[] clouds5;

    public int[] cloudsSize;
    public Transform cloudParent;

    public GameObject[] cloudPrefab;
    public PlayerMove player;
    public int height;

    Color startColor;
    Color nightColor;

    void Awake()
    {
        startColor = new Color(1, 1, 1, 0.82f);
        clouds0 = new GameObject[cloudsSize[0]];
        clouds1 = new GameObject[cloudsSize[1]];
        clouds2 = new GameObject[cloudsSize[2]];
        clouds3 = new GameObject[cloudsSize[3]];
        clouds4 = new GameObject[cloudsSize[4]];
        clouds5 = new GameObject[cloudsSize[5]];
        CreateCloud();
    }
    void OnEnable()
    {
        CloudPosition();
    }
    void CreateCloud()
    {
        for (int i = 0; i < cloudsSize[0]; i++)
        {
            GameObject instantCloud = Instantiate(cloudPrefab[0], cloudParent);
            clouds0[i] = instantCloud;
            instantCloud.SetActive(false);
        }
        for (int i = 0; i < cloudsSize[1]; i++)
        {
            GameObject instantCloud = Instantiate(cloudPrefab[1], cloudParent);
            clouds1[i] = instantCloud;
            instantCloud.SetActive(false);
        }
        for (int i = 0; i < cloudsSize[2]; i++)
        {
            GameObject instantCloud = Instantiate(cloudPrefab[2], cloudParent);
            clouds2[i] = instantCloud;
            instantCloud.SetActive(false);
        }
        for (int i = 0; i < cloudsSize[3]; i++)
        {
            GameObject instantCloud = Instantiate(cloudPrefab[3], cloudParent);
            clouds3[i] = instantCloud;
            instantCloud.SetActive(false);
        }
        for (int i = 0; i < cloudsSize[4]; i++)
        {
            GameObject instantCloud = Instantiate(cloudPrefab[4], cloudParent);
            clouds4[i] = instantCloud;
            instantCloud.SetActive(false);
        }
        for (int i = 0; i < cloudsSize[5]; i++)
        {
            GameObject instantCloud = Instantiate(cloudPrefab[5], cloudParent);
            clouds5[i] = instantCloud;
            instantCloud.SetActive(false);
        }
    }
    void CloudPosition()
    {
        Color color;        
        
        if (ColorUtility.TryParseHtmlString("#5E85AA", out color))
        {
            nightColor = color;
        }
        
        for (int i = 0; i < cloudsSize[0]; i++)
        {
            int randPosX = Random.Range(-10, 10);
            int randPosY = Random.Range(1000 * height, 1000 * (height + 1));
            clouds0[i].transform.position = new Vector2(randPosX, randPosY);
            if (player.Level % 7 == 3 || player.Level % 7 == 4 || player.Level % 7 == 5 || player.Level % 7 == 6)
            {
                clouds0[i].GetComponent<SpriteRenderer>().color = nightColor;
            }else
            {
                clouds0[i].GetComponent<SpriteRenderer>().color = startColor;
            }
            clouds0[i].SetActive(true);
        }
        for (int i = 0; i < cloudsSize[1]; i++)
        {
            int randPosX = Random.Range(-10, 10);
            int randPosY = Random.Range(1000 * height, 1000 * (height + 1));
            clouds1[i].transform.position = new Vector2(randPosX, randPosY);
            if (player.Level % 7 == 3 || player.Level % 7 == 4 || player.Level % 7 == 5 || player.Level % 7 == 6)
            {
                clouds1[i].GetComponent<SpriteRenderer>().color = nightColor;
            }
            else
            {
                clouds1[i].GetComponent<SpriteRenderer>().color = startColor;
            }
            clouds1[i].SetActive(true);
        }
        for (int i = 0; i < cloudsSize[2]; i++)
        {
            int randPosX = Random.Range(-10, 10);
            int randPosY = Random.Range(1000 * height, 1000 * (height + 1));
            clouds2[i].transform.position = new Vector2(randPosX, randPosY);
            if (player.Level % 7 == 3 || player.Level % 7 == 4 || player.Level % 7 == 5 || player.Level % 7 == 6)
            {
                clouds2[i].GetComponent<SpriteRenderer>().color = nightColor;
            }
            else
            {
                clouds2[i].GetComponent<SpriteRenderer>().color = startColor;
            }
            clouds2[i].SetActive(true);
        }
        for (int i = 0; i < cloudsSize[3]; i++)
        {
            int randPosX = Random.Range(-10, 10);
            int randPosY = Random.Range(1000 * height, 1000 * (height + 1));
            clouds3[i].transform.position = new Vector2(randPosX, randPosY);
            if (player.Level % 7 == 3 || player.Level % 7 == 4 || player.Level % 7 == 5 || player.Level % 7 == 6)
            {
                clouds3[i].GetComponent<SpriteRenderer>().color = nightColor;
            }
            else
            {
                clouds3[i].GetComponent<SpriteRenderer>().color = startColor;
            }
            clouds3[i].SetActive(true);
        }
        for (int i = 0; i < cloudsSize[4]; i++)
        {
            int randPosX = Random.Range(-10, 10);
            int randPosY = Random.Range(1000 * height, 1000 * (height + 1));
            clouds4[i].transform.position = new Vector2(randPosX, randPosY);
            if (player.Level % 7 == 3 || player.Level % 7 == 4 || player.Level % 7 == 5 || player.Level % 7 == 6)
            {
                clouds4[i].GetComponent<SpriteRenderer>().color = nightColor;
            }
            else
            {
                clouds4[i].GetComponent<SpriteRenderer>().color = startColor;
            }
            clouds4[i].SetActive(true);
        }
        for (int i = 0; i < cloudsSize[5]; i++)
        {
            int randPosX = Random.Range(-10, 10);
            int randPosY = Random.Range(1000 * height, 1000 * (height + 1));
            clouds5[i].transform.position = new Vector2(randPosX, randPosY);
            if (player.Level % 7 == 3 || player.Level % 7 == 4 || player.Level % 7 == 5 || player.Level % 7 == 6)
            {
                clouds5[i].GetComponent<SpriteRenderer>().color = nightColor;
            }
            else
            {
                clouds5[i].GetComponent<SpriteRenderer>().color = startColor;
            }
            clouds5[i].SetActive(true);
        }
    }
}
