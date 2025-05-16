using UnityEngine;
using DG.Tweening;
using Hung.UI;

namespace Hung.Gameplay.Dalgona
{
    public class BoxDalgona : MonoBehaviour
    {
        public Dalgona[] list_control_dalgona;
        public GameObject camBox;
        public Transform box_cover;
        public Transform cover_pos_to_move;
        public Dalgona dalgonaChosen;
        public GameObject[] other_boxes;
        public GameObject txt_choose_case;
        Sequence sequence;
        public Ease ease;

        void Start()
        {
            dalgonaChosen = list_control_dalgona[Random.Range(0, list_control_dalgona.Length)];
        }

        public void show_dagona()
        {
            dalgonaChosen.gameObject.SetActive(true);
        }
        public Dalgona get_active_dalgona()
        {
            return dalgonaChosen;
        }

        public void hide_other_boxes()
        {
            for (int i = 0; i < other_boxes.Length; i++)
            {
                other_boxes[i].SetActive(false);
            }
        }

        public void hide_msg_choose_box()
        {
            txt_choose_case.SetActive(false);
        }

        public void move_cam_move_cover()
        {
            sequence = DOTween.Sequence();
            
            camBox.SetActive(true);
            DalgonaController.Instance.camTable.SetActive(false);
            DOVirtual.DelayedCall(2f, delegate
            {
                SoundManager.Instance.PlaySoundOpenBox();
                box_cover.DOMove(cover_pos_to_move.position, .6f).SetEase(ease)
                   .OnComplete(delegate
                   {
                       UIDalgonaController.Instance.guid.SetActive(true);
                   });
            });
        }
    }
}
