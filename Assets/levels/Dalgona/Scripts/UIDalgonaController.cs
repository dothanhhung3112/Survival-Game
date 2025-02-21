using Hung.Gameplay.Dalgona;
using UnityEngine;

namespace Hung.UI
{
    public class UIDalgonaController : MonoBehaviour
    {
        public static UIDalgonaController Instance;
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
                DalgonaController.Instance.canCountTime = true;
                UIMenu.DisplayPanelMenu(false);
                UIGamePlay.DisplayPanelGameplay(true);
                DalgonaController.Instance.StartGame();
        }
    }
}
