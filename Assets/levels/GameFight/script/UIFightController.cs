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
            NativeAdsController.Instance.miniGame = NativeAdsController.MiniGame.GameFight;
            UIMenu.DisplayPanelMenu(true);
            UIMenu.SetActionStartGame(delegate
            {
                UIGamePlay.DisplayPanelGameplay(true);
                GameFightController.Instance.StartGame();
            });

            UIRevive.Instance.SetReviveAction(delegate{
                UIGamePlay.DisplayPanelGameplay(true);
                GameFightController.Instance.Revive();
            });

            UIRevive.Instance.SetOnCloseAction(delegate {
                UILose.DisplayPanelLose(true);
            });
        }
    }
}
