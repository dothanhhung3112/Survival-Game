using ACEPlay.Bridge;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hung.UI
{
    public class UIMenu : MonoBehaviour
    {
        [SerializeField] GameObject menuPanel;
        [SerializeField] TextMeshProUGUI moneyText;
        [SerializeField] TextMeshProUGUI textSeason;
        [SerializeField] Point prefab;
        [SerializeField] RectTransform progressBar;
        List<Point> points = new List<Point>();

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
        }

        public void DisplayPanelMenu(bool enable)
        {
            if (enable)
            {
                if(BridgeController.instance.PlayCount < Manager.Instance.Level)
                {
                    BridgeController.instance.PlayCount = Manager.Instance.Level;  
                }
                menuPanel.SetActive(true);

                if (moneyText) moneyText.text = $"{Manager.Instance.Money}";
                textSeason.text = $"SEASON {Manager.Instance.Season}";
                SetPointsData();
            }
            else
            {
                menuPanel.SetActive(false);
            }
        }

        public void OnClickButtonSetting()
        {
            UISetting.Instance.DisplayPanelSetting(true);
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
