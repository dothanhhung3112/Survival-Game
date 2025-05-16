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

        private void Start()
        {
            NativeAdsController.Instance.miniGame = NativeAdsController.MiniGame.Dalgona;
            UIMenu.DisplayPanelMenu(true);
            UIMenu.SetActionStartGame(delegate
            {
                UIGamePlay.DisplayPanelGameplay(true);
            });

            UIRevive.Instance.SetReviveAction(delegate
            {
                DalgonaController.Instance.Revive();
            });

            UIRevive.Instance.SetOnCloseAction(delegate
            {
                UILose.DisplayPanelLose(true);
            });
        }

        public void OnClickButtonGuid()
        {
            guid.SetActive(false);
            DalgonaController.Instance.StartGame();
            DalgonaController.Instance.canCountTime = true;
        }
    }
}
