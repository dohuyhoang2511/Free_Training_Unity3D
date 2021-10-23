using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text currentCoin;
    public Text runDistance;
    public Text time;

    private int coin;
    private float meter;
    private float timer;
    private static GameManager instance;
    private PlayerMove player;

    public static GameManager Instance => instance;

    private void Awake()
    {
        instance = this;

        player = FindObjectOfType<PlayerMove>();
    }

    // Start is called before the first frame update
    void Start()
    {
        coin = 0;
        currentCoin.text = "x" + coin.ToString();

        Meter();

        timer = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;
        Meter();
        Timing();
    }

    public void AddCoin()
    {
        coin++;

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
}
