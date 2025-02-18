using UnityEngine;

namespace Hung.UI
{
    public class UIGlassSteppingController : MonoBehaviour
    {
        public static UIGlassSteppingController Instance;
        public UIWin UIWin { get { return GetComponentInChildren<UIWin>(); } }
        public UILose UILose { get { return GetComponentInChildren<UILose>(); } }
        public UIGamePlay UIGamePlay { get { return GetComponentInChildren<UIGamePlay>(); } }
        public UIMenu UIMenu { get { return GetComponentInChildren<UIMenu>(); } }
        [SerializeField] Level_4_Controller control_script;
        [SerializeField] Level_4_Timer timer_script;

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
            control_script.game_run = true;
            control_script.start_game = true;
            UIMenu.DisplayPanelMenu(false);
            UIGamePlay.DisplayPanelGameplay(true);
            timer_script.active = true;
        }
    }
}
