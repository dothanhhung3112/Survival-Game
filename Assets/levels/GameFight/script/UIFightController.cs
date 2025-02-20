using UnityEngine;
using Hung.Gameplay.GameFight;
namespace Hung.UI
{
    public class UIFightController : MonoBehaviour
    {
        public static UIFightController Instance;
        public UIWin UIWin { get { return GetComponentInChildren<UIWin>(); } }
        public UILose UILose { get { return GetComponentInChildren<UILose>(); } }
        public UIGamePlay UIGamePlay { get { return GetComponentInChildren<UIGamePlay>(); } }
        public UIMenu UIMenu { get { return GetComponentInChildren<UIMenu>(); } }
        void Awake()
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
            GameFightController.Instance.StartGame();
            UIMenu.DisplayPanelMenu(false);
            UIGamePlay.DisplayPanelGameplay(true);
        }
    }
}
