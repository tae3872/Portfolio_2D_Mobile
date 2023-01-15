using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerUI : MonoBehaviour
{
    public Text nameText;
    public Text itemText;

    // Start is called before the first frame update
    void Start()
    {
        nameText.text = "";
        itemText.text = "";
    }

    public void SetNameText(string _name)
    {
        nameText.text = _name;
    }

    public void SetItemText(string _text)
    {
        itemText.text = _text;
    }
}
[System.Serializable]
public class Winner
{
    public string name;
    public string item;
}
[System.Serializable]
public class GachaItem
{
    public int number;
    public string name;
    public int rate;
}