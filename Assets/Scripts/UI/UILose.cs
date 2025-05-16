using ACEPlay.Bridge;
using UnityEngine;
using UnityEngine.Events;

namespace Hung.UI
{
    public class UILose : MonoBehaviour
    {
        [SerializeField] GameObject losePanel;

        public void DisplayPanelLose(bool enable)
        {
            if (enable)
            {
                BridgeController.instance.PlayCount++;
                NativeAdsController.Instance?.DisplayNativeAdsLose(true);
                BridgeController.instance.rewardedCountOnPlay++;
                losePanel.SetActive(true);
                if (BridgeController.instance.rewardedCountOnPlay >= 3)
                {
                    if (BridgeController.instance.IsRewardReady())
                    {
                        BridgeController.instance.rewardedCountOnPlay = 0;
                        BridgeController.instance.ShowRewarded("reward", null);
                    }
                }
                else
                {
                    UnityEvent eDone = new UnityEvent();
                    eDone.AddListener(() =>
                    {
                        BridgeController.instance.PlayCount = 0;
                        AdBreaks.instance.timeElapsedAdBreak = 0;
                    });
                    BridgeController.instance.ShowInterstitial("lose", null, eDone);
                }
                BridgeController.instance.ShowBannerCollapsible();
            }
            else
            {
                NativeAdsController.Instance?.DisplayNativeAdsLose(false);
                BridgeController.instance.HideBannerCollapsible();
                losePanel.SetActive(false);
            }
        }

        public void OnClickButtonNo()
        {
            Manager.Instance.isRevived = true;
            DisplayPanelLose(false);
            Manager.Instance.LoadNextLevel(false);
            SoundManager.Instance.PlaySoundButtonClick();
        }
    }
}
