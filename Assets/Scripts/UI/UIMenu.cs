using ACEPlay.Bridge;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Hung.UI
{
    public class UIMenu : MonoBehaviour
    {
        [SerializeField] GameObject menuPanel;
        [SerializeField] TextMeshProUGUI moneyText;
        [SerializeField] TextMeshProUGUI textSeason;
        [SerializeField] RectTransform progressBar;
        [SerializeField] Point prefab;
        List<Point> points = new List<Point>();
        public Action startGame;

        private void Awake()
        {
            Transform parent = menuPanel.transform.GetChild(0).GetChild(0);
            int minigameSum = SceneManager.sceneCountInBuildSettings - 1;
            for(int i = 0;i < SceneManager.sceneCountInBuildSettings - 1; i++)
            {
                points.Add(Instantiate(prefab, parent));
            }
            progressBar.sizeDelta = new Vector2(100 * minigameSum,progressBar.sizeDelta.y);
        }

        private void Start()
        {
            BridgeController.instance.ShowBanner();
            AdBreaks.instance.StartCountDownShowAdBreaks();
            SetPointsData();
        }

        public void DisplayPanelMenu(bool enable)
        {
            if (enable)
            {
                BridgeController.instance.ShowBannerCollapsible();
                NativeAdsController.Instance?.DisplayNativeAdsDefault(true);
                if (BridgeController.instance.currentLevel < Manager.Instance.Level)
                {
                    BridgeController.instance.currentLevel = Manager.Instance.Level;  
                }
                menuPanel.SetActive(true);

                if (moneyText) moneyText.text = $"{Manager.Instance.Money}";
                textSeason.text = $"SEASON {Manager.Instance.Season}";
                SetPointsData();
            }
            else
            {
                NativeAdsController.Instance?.DisplayNativeAdsDefault(false);
                BridgeController.instance.HideBannerCollapsible();
                menuPanel.SetActive(false);
            }
        }

        public void OnClickButtonStart()
        {
            BridgeController.instance.PlayCount++;
            if (BridgeController.instance.IsShowAdsPlay)
            {
                UnityEvent e = new UnityEvent();
                e.AddListener(() =>
                {
                    // luồng game sau khi tắt quảng cáo
                    startGame?.Invoke();
                    SoundManager.Instance.PlaySoundButtonClick();
                    DisplayPanelMenu(false);
                });
                UnityEvent eDone = new UnityEvent();
                eDone.AddListener(() =>
                {
                    BridgeController.instance.PlayCount = 0;
                    AdBreaks.instance.timeElapsedAdBreak = 0;
                });
                BridgeController.instance.ShowInterstitial("startgame", e,eDone);
            }
            else
            {
                startGame?.Invoke();
                SoundManager.Instance.PlaySoundButtonClick();
                DisplayPanelMenu(false);
            }
        }

        public void SetActionStartGame(Action startGameAction)
        {
            startGame = startGameAction;    
        }

        public void OnClickButtonSetting()
        {
            UISetting.Instance.DisplayPanelSetting(true);
            SoundManager.Instance.PlaySoundButtonClick();
        }

        public void OnClickButtonShop()
        {
            
        }

        public void AddListenerButtonSupport(UnityAction action)
        {
            //buttonSupport?.AddListener(action);
        }

        void SetPointsData()
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (Manager.Instance.levelLoseList.Contains(i + 1))
                {
                    points[i].SetData(Point.PointState.Lose);
                }
                else if (Manager.Instance.levelWinList.Contains(i + 1))
                {
                    points[i].SetData(Point.PointState.Win, i + 1);
                }
                else if (i + 1 == Manager.Instance.CurrentLevel)
                {
                    points[i].SetData(Point.PointState.Current, i + 1);
                }
                else
                {
                    points[i].SetData(Point.PointState.Lock);
                }
            }
        }
    }
}
