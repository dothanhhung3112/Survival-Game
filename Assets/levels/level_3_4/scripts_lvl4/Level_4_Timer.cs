using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Level_4_Timer : MonoBehaviour
{
    public GameObject lose_panel , InGame_panel;

    public bool active;
    public TextMeshProUGUI text_timer;
    public float total_time , max_time , timer;
    Level_4_Controller control_script;

    // Start is called before the first frame update
    void Start()
    {
        control_script = FindObjectOfType<Level_4_Controller>();

        text_timer.text = total_time.ToString();
    }

    // Update is called once per frame
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
        print("lose timer");
        control_script.break_all_glasses_and_player_fall_timeOut();

        yield return new WaitForSeconds(3.5f);

        lose_panel.SetActive(true);
        InGame_panel.SetActive(false);
    }
}
