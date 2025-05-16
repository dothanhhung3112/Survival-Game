using UnityEngine;
using Hung.Gameplay.Marble;
namespace Hung.UI
{
    public class UIMarbleController : MonoBehaviour
    {
        public static UIMarbleController Instance;
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
            NativeAdsController.Instance.miniGame = NativeAdsController.MiniGame.MarbleShoot;
            UIMenu.DisplayPanelMenu(true);
            UIMenu.SetActionStartGame(delegate
            {
                UIGamePlay.DisplayPanelGameplay(true);
                MarbleGameController.Instance.StartGame();
            });

            UIRevive.Instance.SetReviveAction(delegate
            {
                UIGamePlay.DisplayPanelGameplay(true);
                MarbleGameController.Instance.Revive();
            });

            UIRevive.Instance.SetOnCloseAction(delegate
            {
                UILose.DisplayPanelLose(true);
            });
        }
    }
}
