using UnityEngine;

namespace Hung.UI
{
    public class UIWin : MonoBehaviour
    {
        [SerializeField] GameObject winPanel;

        public void DisplayPanelWin(bool enable)
        {
            if (enable)
            {
                winPanel.SetActive(true);
            }
            else
            {
                winPanel.SetActive(false);
            }
        }

        public void OnCLickButtonWatchAds()
        {
            Manager.Instance.LoadNextLevel();
        }

        public void OnClickButtonClaim()
        {
            Manager.Instance.LoadNextLevel();
        }
    }
}
