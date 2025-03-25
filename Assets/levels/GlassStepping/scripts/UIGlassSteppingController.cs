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

        public void StartButton()
        {
            UIMenu.DisplayPanelMenu(false);
            UIGamePlay.DisplayPanelGameplay(true);
            GlassSteppingController.instance.StartGame();
        }

        public void OnClickButtonGuid()
        {
            guid.SetActive(false);  
            GlassSteppingController.instance.game_run = true;
            GlassSteppingController.instance.start_game = true;
            GlassSteppingController.instance.canCountTime = true;
        }
    }
}
