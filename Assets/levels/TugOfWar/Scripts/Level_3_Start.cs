using UnityEngine;

namespace Hung.Gameplay.TugOfWar
{
    public class Level_3_Start : MonoBehaviour
    {
        public GameObject panel_arrow;
        public GameObject panel_start;
        Level_3_Conroller control_script;

        // Start is called before the first frame update
        void Start()
        {
            control_script = FindObjectOfType<Level_3_Conroller>();
        }

        public void btn_start()
        {
            control_script.game_run = true;
            panel_arrow.SetActive(true);
            panel_start.SetActive(false);

        }
    }
}
