using Hung.UI;
using System.Collections;
using UnityEngine;

namespace Hung.Gameplay.Dalgona
{
    public class DalgonaController : MonoBehaviour
    {
        public static DalgonaController Instance;
        public bool active;
        public ParticleSystem effectDalgona;
        public bool canCountTime = false;
        public DalgonaCam dalgonaCam;
        public LayerMask box_layer;
        public Dalgona dalgonaChosen;
        public BoxDalgona boxDalgona;
        public float total_time, max_time, timer;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            SoundManager.Instance.PlayBGMusic4();
            UIDalgonaController.Instance.UIGamePlay.SetTimeText(total_time);
        }

        void Update()
        {
            if (canCountTime)
            {
                if (timer >= max_time)
                {
                    total_time -= 1f;
                    UIDalgonaController.Instance.UIGamePlay.SetTimeText(total_time);
                    timer = 0f;
                }
                else
                {
                    timer += Time.deltaTime;
                }
                if (total_time <= 0)
                {
                    total_time = 0f;
                    UIDalgonaController.Instance.UIGamePlay.SetTimeText(total_time);
                    canCountTime = false;
                    StartCoroutine(show_lose_panel());
                }
            }

            if (!active) return;
            if (Input.GetMouseButtonDown(0))
            {
                choose_box();
            }
        }

        void choose_box()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, box_layer))
            {
                active = false;
                boxDalgona = hit.collider.GetComponent<BoxDalgona>();
                boxDalgona.hide_other_boxes();
                boxDalgona.hide_msg_choose_box();
                dalgonaChosen = boxDalgona.get_active_dalgona();
                boxDalgona.show_dagona();
                boxDalgona.move_cam_move_cover();
            }
        }

        public void StartGame()
        {
            dalgonaChosen.active = true;
        }

        IEnumerator show_lose_panel()
        {
            DalgonaCam cam_script = FindObjectOfType<DalgonaCam>();
            SoundManager.Instance.StopMusic();
            cam_script.lose_move();
            yield return new WaitForSeconds(3.5f);
            UIDalgonaController.Instance.UIGamePlay.DisplayPanelGameplay(false);
            UIDalgonaController.Instance.UILose.DisplayPanelLose(true);
        }
    }
}
