using UnityEngine;

namespace Hung.UI
{
    public class UILose : MonoBehaviour
    {
        [SerializeField] GameObject losePanel;

        public void DisplayPanelLose(bool enable)
        {
            if (enable)
            {
                losePanel.SetActive(true);
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
            Manager.Instance.LoadNextLevel(false);
        }
    }
}
