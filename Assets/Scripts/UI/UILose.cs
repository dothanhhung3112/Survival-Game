using ACEPlay.Bridge;
using ACEPlay.Native;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Hung.UI
{
    public class UILose : MonoBehaviour
    {
        [SerializeField] GameObject losePanel;

        public void DisplayPanelLose(bool enable)
        {
            if (enable)
            {
                NativeAds.instance.DisplayNativeAds(true);
                BridgeController.instance.rewardedCountOnPlay++;
                BridgeController.instance.ShowBannerCollapsible();
                if (BridgeController.instance.rewardedCountOnPlay >= 3)
                {
                    if (BridgeController.instance.IsRewardReady())
                    {
                        BridgeController.instance.rewardedCountOnPlay = 0;
                        BridgeController.instance.ShowRewarded("reward", null);
                    }
                    losePanel.SetActive(true);
                }
                else
                {
                    UnityEvent e = new UnityEvent();
                    e.AddListener(() =>
                    {
                        losePanel.SetActive(true);
                    });
                    BridgeController.instance.ShowInterstitial("lose", e);
                }
            }
            else
            {
                losePanel.SetActive(false);
            }
        }

        public void OnCLickButtonWatchAdRetry()
        {

        }

        public void OnClickButtonNo()
        {
            NativeAds.instance.DisplayNativeAds(false);
            Manager.Instance.LoadNextLevel(false);
        }
    }
}
