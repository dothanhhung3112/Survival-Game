using ACEPlay.Bridge;
using ACEPlay.Native;
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
                NativeAds.instance.DisplayNativeAds(true);
                moneyText.text = $"{Manager.Instance.Money}";
                BridgeController.instance.rewardedCountOnPlay++;
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
            Manager.Instance.Money += 1000;
            MoneySpawner.Instance.SpawnCoin(curMoney, curMoney + 1000, moneyText);
            DOVirtual.DelayedCall(1.5f,delegate
            {
                NativeAds.instance.DisplayNativeAds(false);
                Manager.Instance.LoadNextLevel(true);
            });
        }
    }
}
