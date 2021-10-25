using System.Collections;
using System.Collections.Generic;
using TigerForge;
using UnityEngine;
using UnityEngine.UI;

public class UIShopElement : MonoBehaviour
{
    [SerializeField] private int id;
    public List<int> cost;

    public Text myCoin;
    public Text costTxt;
    public GameObject coinImage;
    public Button purchaseBtn;
    public GameObject purchaseButton;

    private Presenter presenter;
    private UIManager uiManager;

    private void Awake()
    {
        presenter = GameObject.FindObjectOfType<Presenter>();
        uiManager = GameObject.FindObjectOfType<UIManager>();

        GetPlayerId();
        UpdateView();
        UpdateMyCoin();
    }

    private void GetPlayerId()
    {
        id = presenter.GetPlayerId();
    }

    private void UpdateView()
    {
        //User co dang so huu player hay khong?
        var isOwned = DataPlayer.IsOwnedPlayerWithId(id);

        //Neu so huu thi khong cho mua va nguoc lai
        if (isOwned)
        {
            purchaseButton.SetActive(false);
            purchaseBtn.enabled = false;
            coinImage.SetActive(false);
            costTxt.text = "Owned";
        }
        else
        {
            purchaseButton.SetActive(true);
            purchaseBtn.enabled = true;
            coinImage.SetActive(true);
            costTxt.text = cost[id].ToString();
        }
    }

    private void UpdateMyCoin()
    {
        myCoin.text = "My Coin: " + DataPlayer.GetCoin().ToString();
    }

    public void NextButton()
    {
        //SoundManager.Instance.OnPlayButtonClip();
        SoundManager.Instance.OnPlaySound(SoundType.button);

        uiManager.SetPlayerState(false);
        presenter.CheckIdToRight();
        GetPlayerId();
        presenter.SetPlayer();
        UpdateView();
        uiManager.SetPlayerState(true);
    }

    public void PreviousButton()
    {
        //SoundManager.Instance.OnPlayButtonClip();
        SoundManager.Instance.OnPlaySound(SoundType.button);

        uiManager.SetPlayerState(false);
        presenter.CheckIdToLeft();
        GetPlayerId();
        presenter.SetPlayer();
        UpdateView();
        uiManager.SetPlayerState(true);
    }

    public void PurchaseButton()
    {
        //SoundManager.Instance.OnPlayButtonClip();
        SoundManager.Instance.OnPlaySound(SoundType.button);

        var canPurchase = DataPlayer.IsEnoughMoney(cost[id]);

        if (canPurchase)
        {
            DataPlayer.AddPlayer(id);
            UpdateView();

            DataPlayer.SubCoin(cost[id]);
            UpdateMyCoin();
        }
    }
}
