using ACEPlay.Bridge;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NoAds : MonoBehaviour
{
    public static NoAds instance;

    [SerializeField] string keyNoAds;
    [SerializeField] GameObject panelNoads;
    [SerializeField] Transform popUp;
    [SerializeField] GameObject openRemoveAdsButton;
    [SerializeField] GameObject RemoveAdsFiveButton;
    [SerializeField] TextMeshProUGUI textRewardCount;
    [SerializeField] TextMeshProUGUI textTimeLeft;
    float rewardCount;

    public bool IsBuyedRemoveAds
    {
        get { return PlayerPrefs.GetInt("IsBuyedRemoveAds", 0) == 0 ? false : true; }
        set { PlayerPrefs.SetInt("IsBuyedRemoveAds", value ? 1 : 0); }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        if (!IsBuyedRemoveAds)
        {
            //openRemoveAdsButton.SetActive(BridgeController.instance.IsBannerShowing);
        }
    }

    public void DisplayNoAdsDialog(bool enable)
    {
        if (enable)
        {
            Time.timeScale = 0;
            panelNoads.SetActive(enable);
            popUp.DOPunchScale(Vector3.one * 0.03f, 0.2f, 20, 1).SetUpdate(true);
        }
        else
        {
            Time.timeScale = 1;
            panelNoads.SetActive(false);
        }
    }

    public void OnClickButtonNoadsFive()
    {
        UnityEvent e = new UnityEvent();
        e.AddListener(delegate
        {
            rewardCount++;
            if (rewardCount == 2)
            {
                rewardCount = 0;
                StartCoroutine(TimeNoAdsCounting());
            }
            textRewardCount.text = $"{rewardCount}/2";
        });
        BridgeController.instance.ShowRewarded("noadfive", e);
    }

    public void OnClickButtonNoads()
    {
        UnityStringEvent e = new UnityStringEvent();
        e.AddListener((result) =>
        {
            // phần thưởng trong gói mà user đã mua
            IsBuyedRemoveAds = true;
            BridgeController.instance.CanShowAds = false;
        });
        BridgeController.instance.PurchaseProduct(Application.identifier + "_removeads", e);
    }

    public void OnClickButtonClose()
    {
        DisplayNoAdsDialog(false);
    }

    IEnumerator TimeNoAdsCounting()
    {
        RemoveAdsFiveButton.SetActive(false);
        textTimeLeft.gameObject.SetActive(true);
        BridgeController.instance.CanShowInterIngame = false;
        float time = 300f;
        while (time > 0)
        {
            time -= Time.unscaledDeltaTime;
            int minute = (int)time / 60;
            int second = (int)time % 60;
            string formattedTime = string.Format("{0:D2}:{1:D2}", minute, second);
            textTimeLeft.text = $"NoAds: {formattedTime}/5";
            yield return null;
        }
        RemoveAdsFiveButton.SetActive(true);
        textTimeLeft.gameObject.SetActive(false);

        BridgeController.instance.CanShowInterIngame = true;
    }
}
