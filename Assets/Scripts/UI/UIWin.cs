using ACEPlay.Bridge;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Hung.UI
{
    public class UIWin : MonoBehaviour
    {
        [SerializeField] GameObject winPanel;
        [SerializeField] TextMeshProUGUI moneyText;
        [SerializeField] GameObject buttonClaim;

        public void DisplayPanelWin(bool enable)
        {
            if (enable)
            {
                BridgeController.instance.rewardedCountOnPlay++;
                BridgeController.instance.PlayCount++;
                AdBreaks.instance.StopCountDownShowAdBreaks();
                NativeAdsController.Instance?.DisplayNativeAdsDefault(true);

                moneyText.text = $"{Manager.Instance.Money}";
                winPanel.SetActive(true);
                if (BridgeController.instance.rewardedCountOnPlay >= 3)
                {
                    if (BridgeController.instance.IsRewardReady())
                    {
                        BridgeController.instance.rewardedCountOnPlay = 0;
                        BridgeController.instance.ShowRewarded("reward", null);
                    }
                    PiggyBankWin.Instance.StartSpawnMoney();
                }
                else
                {
                    UnityEvent e = new UnityEvent();
                    e.AddListener(() =>
                    {
                        PiggyBankWin.Instance.StartSpawnMoney();
                    });
                    UnityEvent eDone = new UnityEvent();
                    eDone.AddListener(() =>
                    {
                        BridgeController.instance.PlayCount = 0;
                        AdBreaks.instance.timeElapsedAdBreak = 0;
                    });
                    BridgeController.instance.ShowInterstitial("win", e, eDone);
                }
                BridgeController.instance.ShowBannerCollapsible();
            }
            else
            {
                NativeAdsController.Instance?.DisplayNativeAdsDefault(false);
                BridgeController.instance.HideBannerCollapsible();
                winPanel.SetActive(false);
            }
        }

        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        int curMoney = Manager.Instance.Money;
        //        MoneySpawner.Instance.SpawnCoin(curMoney, curMoney + 1000, moneyText);
        //    }
        //}

        public void OnCLickButtonWatchAds()
        {
            Manager.Instance.LoadNextLevel(true);
        }

        public void OnClickButtonClaim()
        {
            buttonClaim.SetActive(false);
            int curMoney = Manager.Instance.Money;
            Manager.Instance.Money += 300;
            MoneySpawner.Instance.SpawnCoin(curMoney, curMoney + 300, moneyText);
            DOVirtual.DelayedCall(2f,delegate
            {
                DisplayPanelWin(false);
                Manager.Instance.LoadNextLevel(true);
            });
            SoundManager.Instance.PlaySoundButtonClick();
        }
    }
}
