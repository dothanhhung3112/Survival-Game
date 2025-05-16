using ACEPlay.Bridge;
using DG.Tweening;
using Hung;
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


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (BridgeController.instance.CheckOwnerNonConsumable(Application.identifier + "_removeads"))
        {
            BridgeController.instance.CanShowAds = false;
        }
        BridgeController.instance.onBannerState += DisplayOpenRemoveAdsButton;
    }

    public void DisplayNoAdsDialog(bool enable)
    {
        if (enable)
        {
            Time.timeScale = 0;
            panelNoads.SetActive(enable);
            popUp.DOPunchScale(Vector3.one * 0.03f, 0.2f, 20, 1).SetUpdate(true);
            BridgeController.instance.ShowBannerCollapsible();
        } 
        else
        {
            Time.timeScale = 1;
            panelNoads.SetActive(false);
            BridgeController.instance.HideBannerCollapsible();
        }
        openRemoveAdsButton.SetActive(!enable);
    }

    void DisplayOpenRemoveAdsButton(bool IsBannerShowing)
    {
        openRemoveAdsButton.SetActive(IsBannerShowing);
    }

    private void OnDestroy()
    {
        BridgeController.instance.onBannerState -= DisplayOpenRemoveAdsButton;
    }

    public void OnClickButtonNoadsFive()
    {
        SoundManager.Instance.PlaySoundButtonClick();
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
        SoundManager.Instance.PlaySoundButtonClick();
        UnityStringEvent e = new UnityStringEvent();
        e.AddListener((result) =>
        {
            // phần thưởng trong gói mà user đã mua
            BridgeController.instance.CanShowAds = false;
        });
        BridgeController.instance.PurchaseProduct(Application.identifier + "_removeads", e);
    }

    public void OnClickButtonOpenNoads()
    {
        DisplayNoAdsDialog(true);
    }

    public void OnClickButtonClose()
    {
        SoundManager.Instance.PlaySoundButtonClick();
        BridgeController.instance.PlayCount++;
        UnityEvent e = new UnityEvent();
        e.AddListener(() =>
        {
            // luồng game sau khi tắt quảng cáo
            DisplayNoAdsDialog(false);
        });
        UnityEvent eDone = new UnityEvent();
        eDone.AddListener(() =>
        {
            BridgeController.instance.PlayCount = 0;
            AdBreaks.instance.timeElapsedAdBreak = 0;
        });
        BridgeController.instance.ShowInterstitial("noads_close", e,eDone);
    }

    IEnumerator TimeNoAdsCounting()
    {
        RemoveAdsFiveButton.SetActive(false);
        textTimeLeft.gameObject.SetActive(true);
        BridgeController.instance.CanShowAds = false;
        BridgeController.instance.HideBanner();
        float time = 300f;
        while (time > 0)
        {
            time -= Time.unscaledDeltaTime;
            int minute = (int)time / 60;
            int second = (int)time % 60;
            string formattedTime = string.Format("{0}:{1:D2}", minute, second);
            textTimeLeft.text = $"NoAds: {formattedTime}/5";
            yield return null;
        }
        RemoveAdsFiveButton.SetActive(true);
        textTimeLeft.gameObject.SetActive(false);
        BridgeController.instance.CanShowAds = true;
        BridgeController.instance.ShowBanner();
    }
}
