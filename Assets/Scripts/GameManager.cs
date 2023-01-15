using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    void Awake()
    {
        if (instance!=null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI winningText;

    public int playerMoney = 1000000;
    public int winnings = 0;
    int jackpot = 5000;
    int playerBet = 10;

    void Start()
    {

    }
    private void Update()
    {
        moneyText.text = $"$ {playerMoney:#,###}";
    }
    public void DisplayWinning()
    {
        winningText.text = winnings.ToString();
    }
    public void ResetWinningDisplay()
    {
        winnings = 0;
        winningText.text = "";
    }
}
