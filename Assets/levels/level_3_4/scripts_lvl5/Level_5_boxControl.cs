using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Level_5_boxControl : MonoBehaviour
{
    public Level_5_Control_Dalgona[] list_control_dalgona;
    public Transform cam_pos;
    public Camera cam;
    public Transform box_cover;
    public Transform cover_pos_to_move;
    public Level_5_Control_Dalgona control_dalgona_chosen;
    public GameObject[] other_boxes;
    public GameObject txt_choose_case;
    Sequence sequence;
    public Ease ease;
    public GameObject panel_timer;

    // Start is called before the first frame update
    void Start()
    {
        control_dalgona_chosen = list_control_dalgona[Random.Range(0, list_control_dalgona.Length)];
        //control_dalgona_chosen.gameObject.SetActive(true);

    }

    public void show_dagona()
    {
        control_dalgona_chosen.gameObject.SetActive(true);
    }
    public Level_5_Control_Dalgona get_active_dalgona()
    {
        return control_dalgona_chosen;
    }

    public void hide_other_boxes()
    {
        for (int i = 0; i < other_boxes.Length; i++)
        {
            other_boxes[i].SetActive(false);
        }
    }

    //public void move_cover()
    //{
    //    box_cover.DOMove(cover_pos_to_move.position, 2f);
    //}

    //public void move_cam_to_chosen_box()
    //{
    //    cam.transform.DOMove(cam_pos.position, 2f);
    //}

    public void hide_msg_choose_box()
    {
        txt_choose_case.SetActive(false);
    }

    public void move_cam_move_cover()
    {
        sequence = DOTween.Sequence();

        sequence

                    .Append(cam.transform.DOMove(cam_pos.position, 1f).SetEase(ease))
                    .Append(box_cover.DOMove(cover_pos_to_move.position, .6f).SetEase(ease))
                    .OnComplete(() => panel_timer.SetActive(true));
    }
}
