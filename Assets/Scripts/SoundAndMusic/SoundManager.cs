using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    button = 0,
    collectCoin = 1,
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource audioFX;

    //public AudioClip buttonClip;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(this);
        }
    }

    //private void OnValidate()
    //{
    //    if(audioFX == null)
    //    {
    //        audioFX = gameObject.AddComponent<AudioSource>();
    //        audioFX.playOnAwake = false;
    //    }
    //}

    //public void OnPlayButtonClip()
    //{
    //    audioFX.PlayOneShot(buttonClip);
    //}

    public void OnPlaySound(SoundType soundType)
    {
        var audio = Resources.Load<AudioClip>($"Sounds/{soundType.ToString()}");

        audioFX.PlayOneShot(audio);
    }
}
