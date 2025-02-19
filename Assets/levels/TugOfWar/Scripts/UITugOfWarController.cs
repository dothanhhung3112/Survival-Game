using Hung.Gameplay.GreenRedLight;
using Hung.UI;
using Hung;
using UnityEngine;
using Hung.Gameplay.TugOfWar;

namespace Hung.UI
{
    public class UITugOfWarController : MonoBehaviour
    {
        public static UITugOfWarController Instance;
        public UIWin UIWin { get { return GetComponentInChildren<UIWin>(); } }
        public UILose UILose { get { return GetComponentInChildren<UILose>(); } }
        public UIGamePlay UIGamePlay { get { return GetComponentInChildren<UIGamePlay>(); } }
        public UIMenu UIMenu { get { return GetComponentInChildren<UIMenu>(); } }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            UIMenu.DisplayPanelMenu(true);
        }

        public void StartButton()
        {
            TugOfWarController.Instance.runnedGame = true;
            UIGamePlay.DisplayPanelGameplay(true);
            UIMenu.DisplayPanelMenu(false);
        }
    }
}