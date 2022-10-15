using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using System;


public class GameManager : MonoBehaviour
{
    public bool isMobile;

    public string[] levelTarget;
    public Text targetNumberUI;
    public PlayerMove player;
    public GameObject[] initGrounds;

    public int Health;
    public Text heartCntUI;
    public Text heightUI;
    public Text jumpCntUI;
    public Text paraUI;

    public Text bestHeightUI;
    public Text curTopUI;
    public Text topJumpCntUI;
    public Scrollbar paraScrollbar;

    float height;
    float bestHeight = 0f;
    float curTopHeights = 0f;
    public int jumpCnt;
    int topJumpcnt;

    public GameObject pauseUI;
    public GameObject overSet;
    public GameObject gameSet;
    public GameObject mobileSet;

    public bool canPress = false;

    public GameObject waterfall;
    public ParticleSystem heartBreakParti;
    public ParticleSystem heartPlusParti;


    [Header("----------Audio")]
    public AudioSource[] sfxPlayer;
    public AudioSource[] bgmPlayer;
    public AudioClip[] sfxClip;
    public AudioClip[] superJumpClip;

    public GameObject[] cloudBacks;

    int sfxPlayerIndex;
    public enum Sfx
    {
        OopsJump, SuperJump, Finish, Attach, Button,
        Parachute, Broken, GameOver, Quit, ButtonReturn, HealthDown, HealthUp
    };
    private RewardedAd rewardedAd;
    void Awake()
    {
        Finish.plusHeight = 0;
        bestHeight = PlayerPrefs.GetFloat("TopHeight", 0);

        Health = PlayerPrefs.GetInt("Health", 5);
        if (Health == 0 && !Admob.instance.isReward)
        {
            Health = 1;
            heartPlusParti.Play();
        }
        else if (Admob.instance.isReward)
        {
            StartCoroutine(HealthParti());
        }

        MobileSet();
        Background();
        LoadAds();

    }
    void Start()
    {
        PlaySound(GameManager.Sfx.Button);
        LoadAds();
    }

    IEnumerator HealthParti()
    {
        Health++;
        heartPlusParti.Play();
        PlaySound(Sfx.HealthUp);
        yield return new WaitForSeconds(0.8f);
        Health++;
        heartPlusParti.Play();
        PlaySound(Sfx.HealthUp);
        yield return new WaitForSeconds(0.8f);
        Health++;
        heartPlusParti.Play();
        PlaySound(Sfx.HealthUp);
        yield return new WaitForSeconds(0.8f);
        Health++;
        heartPlusParti.Play();
        PlaySound(Sfx.HealthUp);
        yield return new WaitForSeconds(0.8f);
        Health++;
        heartPlusParti.Play();
        PlaySound(Sfx.HealthUp);
        PlayerPrefs.SetInt("Health", Health);
        Admob.instance.isReward = false;

    }
    void LoadAds()
    {
        string adUnitId;
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/5224354917";
        //adUnitId = "ca-app-pub-9851803813029032/9610021546";
#elif UNITY_IPHONE
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            adUnitId = "unexpected_platform";
#endif

        this.rewardedAd = new RewardedAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
        //this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
    }


    //public void HandleRewardedAdLoaded(object sender, EventArgs args)
    //{
    //    print("HandleRewardedAdLoaded event received");
    //}
    public void HandleUserEarnedReward(object sender, Reward args)
    {
        Admob.instance.isReward = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Retry()
    {
        StartCoroutine(UserChoseToWatchAd());
    }

    IEnumerator UserChoseToWatchAd()
    {
        while (!this.rewardedAd.IsLoaded())
        {
            yield return new WaitForSeconds(0.2f);
        }
        this.rewardedAd.Show();
    }


    public void Background()
    {
        for (int i = 0; i < (player.levelCycle <= 9 ? player.levelCycle + 1 : 9); i++)
        {
            cloudBacks[i].SetActive(true);
        }
    }
    public void BackgroundHide()
    {
        for (int i = 0; i < (player.levelCycle <= 9 ? player.levelCycle + 1 : 9); i++)
        {
            cloudBacks[i].SetActive(false);

        }
    }


    void SetResolution1()
    {
        int Width = 640;
        int Height = 960;
        Screen.SetResolution(Width, Height, true);
    }
    public void SetResolution2()
    {
        int setWidth = 640; // 사용자 설정 너비
        int setHeight = 960; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
        }
        else // 게임의 해상도 비가 더 큰 경우
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
        }
    }

    private void Update()
    {
        OverUiSet();
        UiSet();
        ParaScroll();
        Pause();

    }
    void MobileSet()
    {
        mobileSet.SetActive(isMobile);
    }
    void UiSet()
    {
        targetNumberUI.text = levelTarget[player.Level % 7];
        heartCntUI.text = Health.ToString();
        paraUI.text = player.parable.ToString("F1");
        heightUI.text = height.ToString("F0") + "m";
    }
    void OverUiSet()
    {
        height = player.transform.position.y + Finish.plusHeight;
        if (height > bestHeight)
        {
            bestHeight = float.Parse(height.ToString("F2"));
            PlayerPrefs.SetFloat("TopHeight", bestHeight);
        }
        if (height > curTopHeights)
        {
            curTopHeights = float.Parse(height.ToString("F2"));
        }
        if (jumpCnt > topJumpcnt)
        {
            topJumpcnt = jumpCnt;
        }
    }
    void ParaScroll()
    {
        if (paraScrollbar.size == 1)
        {
            player.parable++;
            paraScrollbar.size = 0;
        }
        paraScrollbar.size += Time.deltaTime * 0.005f;
    }
    void Pause()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            pauseUI.SetActive(!pauseUI.activeSelf);
            gameSet.SetActive(!pauseUI.activeSelf);
            mobileSet.SetActive(!pauseUI.activeSelf);
            Time.timeScale = pauseUI.activeSelf ? 0 : 1;
            PlaySound(pauseUI.activeSelf ? Sfx.Button : Sfx.ButtonReturn);
        }
    }
    public void PauseMobile()
    {
        pauseUI.SetActive(!pauseUI.activeSelf);
        gameSet.SetActive(!pauseUI.activeSelf);
        mobileSet.SetActive(!pauseUI.activeSelf);
        Time.timeScale = pauseUI.activeSelf ? 0 : 1;
        PlaySound(pauseUI.activeSelf ? Sfx.Button : Sfx.ButtonReturn);
    }

    public IEnumerator HealthDown()
    {
        if (Health > 1)
        {
            yield return new WaitForSeconds(0.1f);
            player.gameObject.layer = 8;
            Health--;
            PlayerPrefs.SetInt("Health", Health);
            heartBreakParti.Play();
            PlaySound(Sfx.HealthDown);
            yield return new WaitForSeconds(0.5f);
            Reposition();
            PlayerPrefs.SetInt("Health", Health);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
            player.gameObject.layer = 8;
            Health--;
            PlayerPrefs.SetInt("Health", Health);
            heartBreakParti.Play();
            PlaySound(Sfx.GameOver);
            bestHeightUI.text = "Best : " + PlayerPrefs.GetFloat("TopHeight") + "m";
            curTopUI.text = "Top : " + curTopHeights + "m";
            topJumpCntUI.text = "No. Jump : " + topJumpcnt;
            GPGSBinder.Inst.ReportLeaderboard(GPGSIds.leaderboard_bestheight, (long)(PlayerPrefs.GetFloat("TopHeight", 0) * 100));
            yield return new WaitForSeconds(0.5f);
            player.gameObject.SetActive(false);
            overSet.SetActive(true);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(waterfall).transform.position = collision.transform.position;
            player.GetComponent<Animator>().SetBool("reJump", true);
            StartCoroutine(HealthDown());
        }
    }
    void Reposition()
    {
        player.gameObject.layer = 7;

        for (int i = 0; i < initGrounds.Length; i++)
        {
            if (initGrounds[i].activeSelf)
            {
                initGrounds[i].transform.position = new Vector2(0, -0.5f);
                player.transform.position = initGrounds[i].transform.position;
            }
        }
        player.jumpPower = 0;
    }
    public void PlaySound(Sfx type)
    {
        switch (type)
        {
            case Sfx.SuperJump:
                sfxPlayer[sfxPlayerIndex].clip = superJumpClip[player.jumpPower <= 5 ? player.jumpPower : 5];
                sfxPlayer[sfxPlayerIndex].volume = 0.5f;
                break;
            case Sfx.OopsJump:
                sfxPlayer[sfxPlayerIndex].clip = sfxClip[0];
                sfxPlayer[sfxPlayerIndex].volume = 0.2f;
                break;
            case Sfx.Finish:
                sfxPlayer[sfxPlayerIndex].clip = sfxClip[1];
                sfxPlayer[sfxPlayerIndex].volume = 0.5f;
                break;
            case Sfx.Attach:
                sfxPlayer[sfxPlayerIndex].clip = sfxClip[2];
                sfxPlayer[sfxPlayerIndex].volume = 0.3f;
                break;
            case Sfx.Button:
                sfxPlayer[sfxPlayerIndex].clip = sfxClip[3];
                sfxPlayer[sfxPlayerIndex].volume = 0.2f;
                break;
            case Sfx.Parachute:
                sfxPlayer[sfxPlayerIndex].clip = sfxClip[4];
                sfxPlayer[sfxPlayerIndex].volume = 0.4f;
                break;
            case Sfx.GameOver:
                sfxPlayer[sfxPlayerIndex].clip = sfxClip[5];
                sfxPlayer[sfxPlayerIndex].volume = 0.5f;
                break;
            case Sfx.Quit:
                sfxPlayer[sfxPlayerIndex].clip = sfxClip[6];
                sfxPlayer[sfxPlayerIndex].volume = 0.5f;
                break;
            case Sfx.ButtonReturn:
                sfxPlayer[sfxPlayerIndex].clip = sfxClip[7];
                sfxPlayer[sfxPlayerIndex].volume = 0.3f;
                break;
            case Sfx.HealthDown:
                sfxPlayer[sfxPlayerIndex].clip = sfxClip[8];
                sfxPlayer[sfxPlayerIndex].volume = 0.3f;
                break;
            case Sfx.HealthUp:
                sfxPlayer[sfxPlayerIndex].clip = sfxClip[9];
                sfxPlayer[sfxPlayerIndex].volume = 0.3f;
                break;
        }
        sfxPlayer[sfxPlayerIndex].Play();
        sfxPlayerIndex = (sfxPlayerIndex + 1) % sfxPlayer.Length;
    }

    public void StopSound(AudioClip audioClip)
    {
        for (int i = 0; i < sfxPlayer.Length; i++)
        {
            if (sfxPlayer[i].clip == audioClip)
            {
                sfxPlayer[i].Stop();
            }
        }
    }
    public void RestartButton()
    {
        //if (Health == 0)
        //{
        //    Health = 1;
        //    PlayerPrefs.SetInt("Health", Health);
        //}
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        PlaySound(Sfx.Quit);
        Application.Quit();
    }
    public void LogOut()
    {
        PlaySound(Sfx.Button);
        GPGSBinder.Inst.Logout();
    }
    public void RankingLoading()
    {
        PlaySound(Sfx.Button);
        GPGSBinder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_bestheight);
    }
    public void RankingLoaded()
    {
        PlaySound(Sfx.Button);
        GPGSBinder.Inst.ShowTargetLeaderboardUI(GPGSIds.leaderboard_bestheight);
    }
}
