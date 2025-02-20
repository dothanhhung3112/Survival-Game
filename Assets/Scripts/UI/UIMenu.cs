using TMPro;
using UnityEngine;

namespace Hung.UI
{
    public class UIMenu : MonoBehaviour
    {
        [SerializeField] GameObject menuPanel;
        [SerializeField] TextMeshProUGUI textSeason;
        [SerializeField] Point[] points;

        public void DisplayPanelMenu(bool enable)
        {
            if (enable)
            {
                menuPanel.SetActive(true);
                textSeason.text = $"SEASON {Manager.Instance.Season}";
                SetPointsData();
            }
            else
            {
                menuPanel.SetActive(false);
            }
        }

        void SetPointsData()
        {
            for (int i = 0; i < points.Length; i++)
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
