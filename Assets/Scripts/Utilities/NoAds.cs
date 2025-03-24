using ACEPlay.Bridge;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NoAds : MonoBehaviour
{
    public static NoAds instance;
    [SerializeField] string keyNoAds;
    [SerializeField] GameObject panelNoads;
    [SerializeField] TextMeshProUGUI textRewardCount;
    [SerializeField] TextMeshProUGUI textTimeLeft;
    float rewardCount;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void DisplayNoAdsDialog(bool enable)
    {
        panelNoads.SetActive(enable);
    }

    public void OnClickButtonNoadsFive()
    {
        UnityEvent e = new UnityEvent();
        e.AddListener(delegate
        {
            rewardCount++;
            if(rewardCount == 2)
            {
                rewardCount = 0;
                BridgeController.instance.CanShowAds = false;
            }
        });
        BridgeController.instance.ShowRewarded("noadfive", e);
    }

    public void OnClickButtonNoads()
    {
        UnityStringEvent e = new UnityStringEvent();
        e.AddListener((result) =>
        {
            // phần thưởng trong gói mà user đã mua
            BridgeController.instance.CanShowAds = false;
        });
        BridgeController.instance.PurchaseProduct(Application.identifier + "_removeads", e);
    }

    public void OnClickButtonClose()
    {
        panelNoads.SetActive(false);
    }
}
