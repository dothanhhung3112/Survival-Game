using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Level_5_timer : MonoBehaviour
{
    public GameObject lose_panel, InGame_panel;

    public bool active;
    public TextMeshProUGUI text_timer;
    public float total_time, max_time, timer;
    Level_5_controller control_script;
    public GameObject guide , btn_start;

    // Start is called before the first frame update
    void Start()
    {
        control_script = FindObjectOfType<Level_5_controller>();

        text_timer.text = total_time.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (timer >= max_time)
            {
                total_time -= 1f;
                text_timer.text = total_time.ToString();
                timer = 0f;
            }
            else
            {
                timer += Time.deltaTime;
            }

            if (total_time <= 0)
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
        print("lose timeout");
        //msg timeout

        Level_5_cam cam_script = FindObjectOfType<Level_5_cam>();
        cam_script.lose_move();


        yield return new WaitForSeconds(3.5f);

        lose_panel.SetActive(true);
        InGame_panel.SetActive(false);

        Level_5_controller controller_script = FindObjectOfType<Level_5_controller>();
        controller_script.show_lose_panel();
    }

    public void btn_start_game()
    {
        //if (BridgeController.instance.IsShowAdsPlay)
        //{
        //    UnityEvent e = new UnityEvent();
        //    e.AddListener(delegate
        //    {
        //        active = true;
        //        btn_start.SetActive(false);
        //        guide.SetActive(false);

        //        control_script.start_game();
        //    });
        //    BridgeController.instance.ShowInterstitial("start_game", e);
        //}
        //else
        //{
        //    active = true;
        //    btn_start.SetActive(false);
        //    guide.SetActive(false);

        //    control_script.start_game();
        //}

        
    }
}
