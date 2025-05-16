using UnityEngine;
using Hung.Gameplay.GlassStepping;

namespace Hung.UI
{
    public class UIGlassSteppingController : MonoBehaviour
    {
        public static UIGlassSteppingController Instance;
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
            NativeAdsController.Instance.miniGame = NativeAdsController.MiniGame.GlassStepping;
            UIMenu.SetActionStartGame(delegate
            {
                UIGamePlay.DisplayPanelGameplay(true);
                GlassSteppingController.instance.StartGame();
            });

            UIRevive.Instance.SetReviveAction(delegate
            {
                UIGamePlay.DisplayPanelGameplay(true);
                GlassSteppingController.instance.Revive();
            });

            UIRevive.Instance.SetOnCloseAction(delegate
            {
                UILose.DisplayPanelLose(true);
            });
        }

        public void OnClickButtonGuid()
        {
            guid.SetActive(false);  
            GlassSteppingController.instance.gameRun = true;
            GlassSteppingController.instance.canCountTime = true;
        }
    }
}
