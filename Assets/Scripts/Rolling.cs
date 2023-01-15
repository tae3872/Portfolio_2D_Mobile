using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Rolling : MonoBehaviour
{
    public GameObject[] slotObjs;
    public GameObject icon;
    //public Button[] slots;
    public Sprite[] itemSprites;
    public AnimationCurve animationCurve;
    public GameObject megaUI;
    public GameObject[] payLine;
    bool megaUIOpen;

    GameManager gameManager;

    [System.Serializable]
    public class Line
    {
        public List<Icon> iconsLine = new List<Icon>();
    }
    public Line[] line;

    [System.Serializable]
    public class Row
    {
        public List<Image> iconsRow = new List<Image>();
    }
    public Row[] row;

    [System.Serializable]
    public class SpriteDumy
    {
        public List<Sprite> sprites = new List<Sprite>();
    }
    public SpriteDumy[] dumy;

    public List<int> startList = new List<int>();

    int itemCnt = 5;
    void Start()
    {
        for (int i = 0; i < itemCnt * slotObjs.Length; i++)
        {
            startList.Add(i);
        }
        //SlotStart();
        CreateSlot();
        gameManager = GameManager.instance;
    }

    void CreateSlot()
    {
        int num = 1;
        for (int i = 0; i < slotObjs.Length; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject iconInstant = Instantiate(icon, slotObjs[i].transform);
                iconInstant.GetComponent<Icon>().slotNumber = num++;
                Image image = iconInstant.GetComponent<Image>();
                row[i].iconsRow.Add(image);
                if (iconInstant.GetComponent<Icon>().slotNumber % 5 == 2)
                {
                    line[0].iconsLine.Add(iconInstant.GetComponent<Icon>());
                }
                else if (iconInstant.GetComponent<Icon>().slotNumber % 5 == 3)
                {
                    line[1].iconsLine.Add(iconInstant.GetComponent<Icon>());
                }
                else if (iconInstant.GetComponent<Icon>().slotNumber % 5 == 4)
                {
                    line[2].iconsLine.Add(iconInstant.GetComponent<Icon>());
                }
            }
        }
    }
    public void SlotStart()
    {
        gameManager.ResetWinningDisplay();
        for (int i = 0; i < slotObjs.Length; i++)
        {
            StartCoroutine(StartSlot(i));
        }
        for (int i = 0; i < line.Length; i++)
        {
            line[i].iconsLine.Clear();
        }
        for (int i = 0; i < slotObjs.Length; i++)
        {
            for (int j = 0; j < itemCnt; j++)
            {
                int randomIndex = UnityEngine.Random.Range(0, itemSprites.Length);
                row[i].iconsRow[j].sprite = itemSprites[randomIndex];
                row[i].iconsRow[j].gameObject.GetComponent<Icon>().iconNumber = randomIndex;
                if (row[i].iconsRow[j].GetComponent<Icon>().slotNumber % 5 == 2)
                {
                    line[0].iconsLine.Add(row[i].iconsRow[j].GetComponent<Icon>());
                    row[i].iconsRow[j].GetComponent<Icon>().sprite = itemSprites[randomIndex];
                }
                else if (row[i].iconsRow[j].GetComponent<Icon>().slotNumber % 5 == 3)
                {
                    line[1].iconsLine.Add(row[i].iconsRow[j].GetComponent<Icon>());
                    row[i].iconsRow[j].GetComponent<Icon>().sprite = itemSprites[randomIndex];
                }
                else if (row[i].iconsRow[j].GetComponent<Icon>().slotNumber % 5 == 4)
                {
                    line[2].iconsLine.Add(row[i].iconsRow[j].GetComponent<Icon>());
                    row[i].iconsRow[j].GetComponent<Icon>().sprite = itemSprites[randomIndex];
                }
            }
        }
        Invoke("Bingo", 3f);
    }
    IEnumerator StartSlot(int slotIndex)
    {
        //yield return new WaitForSecondsRealtime(0.5f);
        for (int i = 0; i < 75; i++)
        {
            slotObjs[slotIndex].transform.localPosition -= new Vector3(0, 65f, 0);
            if (slotObjs[slotIndex].transform.localPosition.y < 65f)
            {
                slotObjs[slotIndex].transform.localPosition += new Vector3(0, 325, 0);
            }
            yield return new WaitForSecondsRealtime(0.02f);
        }
    }
    void Bingo()
    {
        for (int i = 0; i < dumy.Length; i++)
        {
            dumy[i].sprites.Clear();
            for (int j = 0; j < 5; j++)
            {
                dumy[i].sprites.Add(line[i].iconsLine[j].sprite);
            }
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (dumy[i].sprites.Where(x => x.Equals(itemSprites[j])).Count() >= 3)
                {
                    int num = dumy[i].sprites.Where(x => x.Equals(itemSprites[i])).Count();
                    gameManager.winnings += num;
                    StartCoroutine(PayLineActive(payLine[i]));
                }
            }
        }
        gameManager.DisplayWinning();


        //if (iconSprite1.Where(x => x.Equals(itemSprites[2])).Count() >= 3)
        //{
        //    int num = iconSprite1.Where(x => x.Equals(itemSprites[2])).Count();
        //    gameManager.winnings += num;
        //    gameManager.DisplayWinning();
        //    StartCoroutine(PayLineActive(payLine[2]));
        //}
    }
    IEnumerator PayLineActive(GameObject gameObject)
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
