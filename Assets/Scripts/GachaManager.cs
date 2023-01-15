using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaManger : MonoBehaviour
{
    public GachaUI nameGacha;
    public GachaUI itemGacha;

    public GachaType gachaType;

    public List<WinnerUI> winnerList = new List<WinnerUI>();
    private int winnerIndex = -1;

    public Transform winnerParent;
    private GameObject winnerPrefab;

    public GameObject resetButton;

    // Start is called before the first frame update
    void Start()
    {
        //리소스 폴더에서 가져와 Prefab 오브젝트 생성
        winnerPrefab = Resources.Load("Prefabs/WinnerSlot") as GameObject;

        winnerIndex = -1;

        gachaType = GachaType.name;
        nameGacha.ReadyAnimation(true);
        itemGacha.ReadyAnimation(false);
    }

    public void NextGacha(string value)
    {
        if (gachaType == GachaType.name)
        {
            SetWinnerName(value);

            gachaType = GachaType.item;
            itemGacha.ReadyAnimation(true);
        }
        else
        {
            SetWinnerItem(value);

            gachaType = GachaType.name;
            nameGacha.ReadyAnimation(true);

            resetButton.SetActive(true);
        }
    }

    private void SetWinnerName(string _name)
    {
        winnerList[winnerIndex].SetNameText(_name);
    }

    private void SetWinnerItem(string _item)
    {
        winnerList[winnerIndex].SetItemText(_item);
    }

    public void CreatWinnerSlot()
    {
        GameObject slot = Instantiate(winnerPrefab) as GameObject;
        slot.transform.SetParent(winnerParent, false);
        winnerList.Add(slot.GetComponent<WinnerUI>());
        winnerIndex++;
    }

    public void ResetWinner()
    {
        winnerIndex = -1;

        nameGacha.ResetGachaItem();
        itemGacha.ResetGachaItem();

        gachaType = GachaType.name;
        nameGacha.ReadyAnimation(true);
        itemGacha.ReadyAnimation(false);

        foreach (Transform slot in winnerParent)
        {
            GameObject.Destroy(slot.gameObject);
        }
        winnerList.Clear();
    }

}

public enum GachaType
{
    name,
    item,
}