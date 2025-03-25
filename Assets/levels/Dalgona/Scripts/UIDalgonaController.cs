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

        public GameObject guid;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void StartButton()
        {
                UIMenu.DisplayPanelMenu(false);
                UIGamePlay.DisplayPanelGameplay(true);
        }

        public void OnClickButtonGuid()
        {
            guid.SetActive(false);
            DalgonaController.Instance.StartGame();
            DalgonaController.Instance.canCountTime = true;
        }
    }
}
