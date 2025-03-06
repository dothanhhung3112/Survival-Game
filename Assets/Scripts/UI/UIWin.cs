using ACEPlay.Bridge;
using UnityEngine;
using UnityEngine.Events;

namespace Hung.UI
{
    public class UIWin : MonoBehaviour
    {
        [SerializeField] GameObject winPanel;

        public void DisplayPanelWin(bool enable)
        {
            if (enable)
            {
                BridgeController.instance.rewardedCountOnPlay++;
                BridgeController.instance.ShowBannerCollapsible();
                if (BridgeController.instance.rewardedCountOnPlay >= 3)
                {
                    if (BridgeController.instance.IsRewardReady())
                    {
                        BridgeController.instance.rewardedCountOnPlay = 0;
                        BridgeController.instance.ShowRewarded("reward", null);
                    }
                    PiggyBankWin.Instance.StartSpawnMoney();
                    winPanel.SetActive(true);
                }
                else
                {
                    UnityEvent e = new UnityEvent();
                    e.AddListener(() =>
                    {
                        PiggyBankWin.Instance.StartSpawnMoney();
                        winPanel.SetActive(true);
                    });
                    BridgeController.instance.ShowInterstitial("win", e);
                }
            }
            else
            {
                winPanel.SetActive(false);
            }
        }

        public void OnCLickButtonWatchAds()
        {
            Manager.Instance.LoadNextLevel(true);
        }

        public void OnClickButtonClaim()
        {
            Manager.Instance.LoadNextLevel(true);
        }
    }
}
