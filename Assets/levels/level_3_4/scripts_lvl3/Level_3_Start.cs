using UnityEngine;

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
        //if (BridgeController.instance.IsShowAdsPlay)
        //{
        //    UnityEvent e = new UnityEvent();
        //    e.AddListener(delegate
        //    {
        //        control_script.game_run = true;
        //        panel_arrow.SetActive(true);
        //        panel_start.SetActive(false);
        //    });
        //    BridgeController.instance.ShowInterstitial("start_game", e);
        //}
        //else
        //{
        //    control_script.game_run = true;
        //    panel_arrow.SetActive(true);
        //    panel_start.SetActive(false);
        //}
    }
}
