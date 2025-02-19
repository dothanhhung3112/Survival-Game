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

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void StartButton()
        {
            GlassSteppingController.instance.game_run = true;
            GlassSteppingController.instance.start_game = true;
            GlassSteppingController.instance.canCountTime = true;
            UIMenu.DisplayPanelMenu(false);
            UIGamePlay.DisplayPanelGameplay(true);
        }
    }
}
