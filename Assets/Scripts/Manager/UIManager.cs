using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject backGround;
    public GameObject menu;
    public GameObject shop;
    public Animator menuAnimator;
    public Animator shopAnimator;

    [SerializeField] private float timeDelay = 0.3f;

    private void Awake()
    {
        MusicManager.Instance.OnPlayMusic(MusicType.Chunky_Monkey);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetPlayerState(false);
    }

    public void PlayButton()
    {
        //SoundManager.Instance.OnPlayButtonClip();
        SoundManager.Instance.OnPlaySound(SoundType.button);

        Invoke("PlayGame", timeDelay);
    }

    private void PlayGame()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void ShopButton()
    {
        //SoundManager.Instance.OnPlayButtonClip();
        SoundManager.Instance.OnPlaySound(SoundType.button);

        Invoke("OpenShop", timeDelay);
    }
    private void OpenShop()
    {
        menuAnimator.Play("Out");
        backGround.SetActive(false);
        shopAnimator.Play("In");
        Invoke("InvokePlayerIn", 0.9f);
    }

    private void InvokePlayerIn()
    {
        SetPlayerState(true);
    }

    public void SetPlayerState(bool isActive)
    {
        EventManager.EmitEventData(EventName.TRIGGER_PRESENTER, isActive);
    }

    public void QuitButton()
    {
        //SoundManager.Instance.OnPlayButtonClip();
        SoundManager.Instance.OnPlaySound(SoundType.button);

        Invoke("QuitGame", timeDelay);
    }

    private void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void BackButton()
    {
        //SoundManager.Instance.OnPlayButtonClip();
        SoundManager.Instance.OnPlaySound(SoundType.button);

        Invoke("BackToMenu", timeDelay);
    }

    private void BackToMenu()
    {
        backGround.SetActive(true);
        shopAnimator.Play("Out");
        menuAnimator.Play("In");
        SetPlayerState(false);
    }
}
