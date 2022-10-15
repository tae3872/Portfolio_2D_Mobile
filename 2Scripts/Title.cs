using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class Title : MonoBehaviour
{
    public GameObject pressKey;
    public Text topHeightTxt;
    public AudioSource source;    
    void Awake()
    {
        topHeightTxt.text = "Best Height : " + PlayerPrefs.GetFloat("TopHeight", 0) + "m";
        GPGSBinder.Inst.ReportLeaderboard(GPGSIds.leaderboard_bestheight, (long)(PlayerPrefs.GetFloat("TopHeight", 0) * 100));
        GPGSBinder.Inst.LoadCustomLeaderboardArray(GPGSIds.leaderboard_bestheight, 20,
                GooglePlayGames.BasicApi.LeaderboardStart.PlayerCentered, GooglePlayGames.BasicApi.LeaderboardTimeSpan.Daily);
        Invoke("PressKeyShow", 2f);
    }
    void Start()
    {
        GPGSBinder.Inst.Login();
    }

    void PressKeyShow()
    {
        pressKey.SetActive(true);
    }
    public void GameStart()
    {
        SceneManager.LoadScene(1);
    }

    public void Ranking()
    {
        source.Play();
        GPGSBinder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_bestheight);
    }
    public void DeletePrefs()
    {
        source.Play();
        PlayerPrefs.DeleteAll();
        topHeightTxt.text = "Best Height : " + PlayerPrefs.GetFloat("TopHeight", 0) + "m";
    }
}
