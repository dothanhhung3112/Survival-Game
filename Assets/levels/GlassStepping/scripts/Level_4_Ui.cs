using Hung.UI;
using UnityEngine;

public class Level_4_Ui : MonoBehaviour
{
    Level_4_Controller control_script;
    public Level_4_Timer timer_script;
    // Start is called before the first frame update
    void Start()
    {
        control_script = FindObjectOfType<Level_4_Controller>();
    }

    public void btn_start()
    {
        control_script.game_run = true;
        control_script.start_game = true;
        UIGlassSteppingController.Instance.UIMenu.DisplayPanelMenu(false);
        UIGlassSteppingController.Instance.UIGamePlay.DisplayPanelGameplay(true);
        timer_script.active = true;
    }
}
