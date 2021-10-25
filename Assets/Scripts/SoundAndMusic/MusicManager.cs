using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicType
{
    Chunky_Monkey = 0,
    GameOver = 1,
    InfiniteDoors = 2,
    JumpyGame = 3,
    Potato = 4,
    StreetLove = 5,
    Stupid_Dancer = 6,
    SunnyDay = 7,
    Tiny_Blocks = 8,
    Zephyr = 9,
}

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioSource audioFX;

    //public AudioClip buttonClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this);
        }
    }

    public void OnPlayMusic(MusicType musicType)
    {
        SmoothSound(audioFX, 1f, musicType);

        //var audio = Resources.Load<AudioClip>($"Music/{musicType.ToString()}");

        ////audioFX.PlayOneShot(audio);

        //audioFX.PlayDelayed(2f);
        //audioFX.loop = true;
        //audioFX.clip = audio;
        //audioFX.Play();
    }

    public void SmoothSound(AudioSource audioSource, float fadeTime, MusicType musicType)
    {
        StartCoroutine(FadeSoundOn(audioSource, fadeTime, musicType));
    }

    IEnumerator FadeSoundOn(AudioSource audioSource, float fadeTime, MusicType musicType)
    {
        yield return FadeSoundOff(audioSource, fadeTime);

        var audio = Resources.Load<AudioClip>($"Music/{musicType.ToString()}");

        audioFX.PlayDelayed(2f);
        if(musicType == MusicType.GameOver)
        {
            audioFX.loop = false;
        }
        else
        {
            audioFX.loop = true;
        }
        audioFX.clip = audio;
        audioFX.Play();

        var t = 0f;
        while (t < 1)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
            if (audioSource != null)
            {
                audioSource.volume = t;
            }
        }
    }

    IEnumerator FadeSoundOff(AudioSource audioSource, float fadeTime)
    {
        var t = fadeTime;
        while(t > 0)
        {
            yield return new WaitForEndOfFrame();
            t -= Time.deltaTime;
            if(audioSource != null)
            {
                audioSource.volume = t;
            }
        }
    }
}
