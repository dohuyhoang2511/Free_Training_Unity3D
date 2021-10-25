using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text currentCoin;
    public Text runDistance;
    public Text time;
    public Text countdown;
    public Text currentCoinOver;
    public Text runDistanceOver;
    public Text timeOver;
    public GameObject inGame;
    public GameObject gameOver;
    public GameObject pause;

    private int coin;
    private float meter;
    private float timer;
    private static GameManager instance;
    private PlayerMove player;
    private bool checkGameOver;
    private bool start;
    public static GameManager Instance => instance;

    private void Awake()
    {
        instance = this;

        MusicManager.Instance.OnPlayMusic(MusicType.StreetLove);

        player = FindObjectOfType<PlayerMove>();
    }

    // Start is called before the first frame update
    void Start()
    {
        inGame.SetActive(true);
        gameOver.SetActive(false);
        pause.SetActive(false);

        timer = 0;
        coin = 0;
        currentCoin.text = "x" + coin.ToString();
        countdown.text = "";

        Meter();

        player.PlayAnimationIdle();
        start = false;
        StartCoroutine(Countdown(3));
    }

    void Update()
    {
        if (!checkGameOver && start)
        {
            timer += Time.deltaTime;
        }
        Meter();
        Timing();
    }

    public void AddCoin()
    {
        coin++;
        DataPlayer.AddCoin(1);
        currentCoin.text = "x" + coin.ToString();
    }

    void Meter()
    {
        meter = Mathf.Round(player.RunDistance());
        runDistance.text = ":" + meter.ToString() + "m";
    }

    void Timing()
    {
        time.text = ":" + Mathf.Round(timer).ToString() + "s";
    }

    public void GameOver()
    {
        MusicManager.Instance.OnPlayMusic(MusicType.GameOver);

        checkGameOver = true;
        inGame.SetActive(false);
        gameOver.SetActive(true);
        pause.SetActive(false);

        currentCoinOver.text = "x" + coin.ToString();
        runDistanceOver.text = ":" + meter.ToString() + "m";
        timeOver.text = ":" + Mathf.Round(timer).ToString() + "s";
    }

    public void PauseButton()
    {
        //SoundManager.Instance.OnPlayButtonClip();
        SoundManager.Instance.OnPlaySound(SoundType.button);

        inGame.SetActive(true);
        gameOver.SetActive(false);
        pause.SetActive(true);

        PauseGame();
    }

    public void ContinueButton()
    {
        //SoundManager.Instance.OnPlayButtonClip();
        SoundManager.Instance.OnPlaySound(SoundType.button);

        inGame.SetActive(true);
        gameOver.SetActive(false);
        pause.SetActive(false);

        Continue();
    }

    private void PauseGame()
    {
        //SoundManager.Instance.OnPlayButtonClip();
        SoundManager.Instance.OnPlaySound(SoundType.button);

        Time.timeScale = 0;
    }

    private void Continue()
    {
        //SoundManager.Instance.OnPlayButtonClip();
        SoundManager.Instance.OnPlaySound(SoundType.button);

        Time.timeScale = 1;
    }

    public void RestartButton()
    {
        //SoundManager.Instance.OnPlayButtonClip();
        SoundManager.Instance.OnPlaySound(SoundType.button);
        MusicManager.Instance.OnPlayMusic(MusicType.StreetLove);

        Continue();
        SceneManager.LoadScene("PlayScene");
    }

    public void HomeButton()
    {
        //SoundManager.Instance.OnPlayButtonClip();
        SoundManager.Instance.OnPlaySound(SoundType.button);
        MusicManager.Instance.OnPlayMusic(MusicType.Chunky_Monkey);

        Continue();
        SceneManager.LoadScene("MenuScene");
    }

    public void QuitButton()
    {
        //SoundManager.Instance.OnPlayButtonClip();
        SoundManager.Instance.OnPlaySound(SoundType.button);

        Application.Quit();
    }


    //Countdown before start game
    IEnumerator Countdown(int seconds)
    {
        int count = seconds;
        float x = player.moveSpeed;

        List<string> cd = new List<string> { "GO", "1", "2", "3" };

        while (count >= 0)
        {
            // display something...
            player.moveSpeed = 0;
            countdown.text = cd[count];

            yield return new WaitForSeconds(1);
            count--;
        }

        // count down is finished...
        countdown.text = "";
        start = true;
        player.PlayAnimationRun();
        player.moveSpeed = x;
    }
}
