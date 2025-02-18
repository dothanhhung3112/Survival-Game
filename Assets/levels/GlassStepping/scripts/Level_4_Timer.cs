using Hung.UI;
using System.Collections;
using TMPro;
using UnityEngine;


public class Level_4_Timer : MonoBehaviour
{
    public GameObject lose_panel , InGame_panel;

    public bool active;
    public TextMeshProUGUI text_timer;
    public float total_time , max_time , timer;
    Level_4_Controller control_script;

    void Start()
    {
        control_script = FindObjectOfType<Level_4_Controller>();
        text_timer.text = total_time.ToString();
    }

    void Update()
    {
        if (active)
        {
            if(timer >= max_time)
            {
                total_time -= 1f;
                text_timer.text = total_time.ToString();
                timer = 0f;
            }
            else
            {
                timer += Time.deltaTime;
            }

            if(total_time <= 0)
            {
                total_time = 0f;
                text_timer.text = total_time.ToString();
                active = false;

                StartCoroutine(show_lose_panel());
            }
        }
    }

    IEnumerator show_lose_panel()
    {
        control_script.break_all_glasses_and_player_fall_timeOut();
        yield return new WaitForSeconds(3.5f);
        UIGlassSteppingController.Instance.UIGamePlay.DisplayPanelGameplay(false);
        UIGlassSteppingController.Instance.UILose.DisplayPanelLose(true);
    }
}
