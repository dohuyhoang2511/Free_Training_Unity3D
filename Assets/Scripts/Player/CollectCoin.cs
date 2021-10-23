using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    public AudioSource collectCoin;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Coin")
        {
            GameManager.Instance.AddCoin();
            collectCoin.Play();
            Destroy(hit.gameObject);
        }
    }
}
