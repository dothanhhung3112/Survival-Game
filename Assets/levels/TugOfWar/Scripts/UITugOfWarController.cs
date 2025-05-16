using UnityEngine;
using Hung.Gameplay.TugOfWar;
using Hung.Gameplay.Dalgona;

namespace Hung.UI
{
    public class UITugOfWarController : MonoBehaviour
    {
        public static UITugOfWarController Instance;
        public UIWin UIWin { get { return GetComponentInChildren<UIWin>(); } }
        public UILose UILose { get { return GetComponentInChildren<UILose>(); } }
        public UIGamePlay UIGamePlay { get { return GetComponentInChildren<UIGamePlay>(); } }
        public UIMenu UIMenu { get { return GetComponentInChildren<UIMenu>(); } }
        public bool isRevive = false;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            NativeAdsController.Instance.miniGame = NativeAdsController.MiniGame.TugOfWar;
            UIMenu.SetActionStartGame(delegate
            {
                UIGamePlay.DisplayPanelGameplay(true);
                TugOfWarController.Instance.runnedGame = true;
            });

            if(Manager.Instance.isRevived)
            {
                UIMenu.DisplayPanelMenu(false);
                UIGamePlay.DisplayPanelGameplay(true);
                TugOfWarController.Instance.runnedGame = true;
            }
            else
            {
                UIMenu.DisplayPanelMenu(true);
            }

            UIRevive.Instance.SetReviveAction(delegate
            {
                SceneLoader.Instance.LoadSceneByIndex(4);

            });

            UIRevive.Instance.SetOnCloseAction(delegate
            {
                UILose.DisplayPanelLose(true);
            });
        }
    }
}