using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;

public class Presenter : MonoBehaviour
{
    public int playerIndex;
    public GameObject player { private set; get; }
    public GameObject[] playerList;

    private void Start()
    {
        SetPlayer();
        SetPlayerState(false);
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventName.TRIGGER_PRESENTER, OnTrigger);
    }

    private void OnTrigger()
    {
        var isActive = EventManager.GetBool(EventName.TRIGGER_PRESENTER);

        SetPlayerState(isActive);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventName.TRIGGER_PRESENTER, OnTrigger);
    }

    public void SetPlayer()
    {
        player = playerList[playerIndex];
        GetPlayerId();
    }

    public void SetPlayerState(bool isActive)
    {
        player.SetActive(isActive);
    }

    public int GetPlayerId()
    {
        return this.playerIndex;
    }

    public void CheckIdToRight()
    {
        playerIndex++;
        if(playerIndex > playerList.Length - 1)
        {
            playerIndex = 0;
        }
    }

    public void CheckIdToLeft()
    {
        playerIndex--;
        if (playerIndex < 0)
        {
            playerIndex = playerList.Length - 1;
        }
    }
}
