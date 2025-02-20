using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Hung.UI;

namespace Hung.Gameplay.GlassStepping
{
    public class GlassSteppingController : MonoBehaviour
    {
        public static GlassSteppingController instance;

        public Glass[] list_glasses;
        public LayerMask glass_layer, finish_layer;
        public List<MeshRenderer> list_true_glasses;
        public Material[] mats;
        public float tm;
        public Camera cam;
        public int actual_step;
        public bool game_run, active_finish, show_start, start_game;
        public float power_jump, duration, fall_power;
        public float total_time, max_time, timer;
        public bool canCountTime = false;
        public Vector3 mypos;

        Rigidbody rb;
        public Animator anim;
        public GameObject finish_pos;

        //camera
        public Transform my_cam, cam_pos_1, cam_pos_finish;
        Sequence sequence;
        public Ease ease;

        GlassSteppingCam cam_follow;
        public ParticleSystem confetti;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        void Start()
        {
            cam_follow = FindObjectOfType<GlassSteppingCam>();
            rb = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            mypos = transform.position;
            StartCoroutine(manage_glasses(tm));
            UIGlassSteppingController.Instance.UIGamePlay.SetTimeText(total_time);
        }

        void Update()
        {
            if (!game_run || !start_game)
                return;


            if (canCountTime)
            {
                if (timer >= max_time)
                {
                    total_time -= 1f;
                    UIGlassSteppingController.Instance.UIGamePlay.SetTimeText(total_time);
                    timer = 0f;
                }
                else
                {
                    timer += Time.deltaTime;
                }

                if (total_time <= 0)
                {
                    total_time = 0f;
                    UIGlassSteppingController.Instance.UIGamePlay.SetTimeText(total_time);
                    canCountTime = false;

                    StartCoroutine(show_lose_panel());
                }
            }

            if (Input.GetKeyUp(KeyCode.Z))
            {
                //transform.position = mypos;
                on_final();
            }

            if (Input.GetKeyUp(KeyCode.G))
            {
                StartCoroutine(manage_glasses(tm));
            }

            if (Input.GetKeyUp(KeyCode.K))
            {
                get_next_step();
            }

            if (Input.GetMouseButtonDown(0))
            {
                //ray cast
                choose_glass();
            }
        }

        IEnumerator show_lose_panel()
        {
            break_all_glasses_and_player_fall_timeOut();
            yield return new WaitForSeconds(3.5f);
            UIGlassSteppingController.Instance.UIGamePlay.DisplayPanelGameplay(false);
            UIGlassSteppingController.Instance.UILose.DisplayPanelLose(true);
        }

        IEnumerator manage_glasses(float tm)
        {
            actual_step = 0;
            list_true_glasses.Clear();
            // fill all glasses
            for (int i = 0; i < list_glasses.Length; i += 2)
            {
                int rdm = Random.Range(0, 2);

                if (rdm == 0)
                {
                    list_glasses[i].type = glass_type.true_glass;
                    list_glasses[i + 1].type = glass_type.wrong_glass;

                    // add to list true glasses
                    MeshRenderer ms = list_glasses[i].GetComponent<MeshRenderer>();

                    list_true_glasses.Add(ms);

                }
                else
                {
                    if (rdm == 1)
                    {
                        list_glasses[i].type = glass_type.wrong_glass;
                        list_glasses[i + 1].type = glass_type.true_glass;

                        // add to list true glasses
                        MeshRenderer ms = list_glasses[i + 1].GetComponent<MeshRenderer>();

                        list_true_glasses.Add(ms);
                    }
                }
            }
            // make hint by animation

            for (int i = 0; i < list_true_glasses.Count; i++)
            {
                list_true_glasses[i].material = mats[2];

                if (i != 0)
                {
                    list_true_glasses[i - 1].material = mats[0];
                }

                yield return new WaitForSeconds(tm);
                if (i == list_true_glasses.Count - 1)
                {
                    list_true_glasses[i].material = mats[0];
                }
            }

            // show all answer and hide them again
            //show
            for (int i = 0; i < list_true_glasses.Count; i++)
            {
                list_true_glasses[i].material = mats[2];

            }
            yield return new WaitForSeconds(1f);
            //hide
            for (int i = 0; i < list_true_glasses.Count; i++)
            {
                list_true_glasses[i].material = mats[0];

            }

            // run the game
            //game_run = true;



            // camera 
            cam_to_player_pos();
            // start steps 
            //get_next_step();

        }

        public void get_next_step()
        {
            if (!game_run)
                return;

            if (!show_start)
            {
                show_start = true;
                UIGlassSteppingController.Instance.UIMenu.DisplayPanelMenu(true);
            }
            //active follow cam
            if (!cam_follow.is_active)
                cam_follow.start_follow();

            if (actual_step == 10)
            {
                active_finish = true;
                finish_pos.SetActive(true);

                //reset all glasses material
                for (int i = 0; i < list_glasses.Length; i++)
                {
                    list_glasses[i].GetComponent<MeshRenderer>().material = mats[0];
                    list_glasses[i].is_active = false;
                }
            }
            else
            {
                // idle animation
                anim.Play("idle_glass");

                //reset all glasses material
                for (int i = 0; i < list_glasses.Length; i++)
                {
                    list_glasses[i].GetComponent<MeshRenderer>().material = mats[0];
                    list_glasses[i].is_active = false;
                }
                // change materials of next step
                int step = actual_step * 2;
                int cnt = 0;
                for (int i = step; i < list_glasses.Length; i++)
                {
                    list_glasses[i].GetComponent<MeshRenderer>().material = mats[1];
                    list_glasses[i].is_active = true;
                    cnt++;
                    if (cnt >= 2)
                        break;
                }
                actual_step++;
            }
        }

        public void choose_glass()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, glass_layer))
            {
                Glass glass = hit.collider.GetComponent<Glass>();
                SoundManager.Instance.PLaySoundJump();
                if (glass.is_active)
                {
                    if (glass.type == glass_type.true_glass)
                    {
                        //get_next_step();
                        //get jump position
                        Vector3 jump_pos = glass.pos_jumping.transform.position;

                        jumping(jump_pos, true);
                    }
                    else if (glass.type == glass_type.wrong_glass)
                    {
                        //get jump position
                        Vector3 jump_pos = glass.pos_jumping.transform.position;
                        jumping(jump_pos, false, glass);

                    }
                }

            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, finish_layer))
            {
                SoundManager.Instance.PLaySoundJump();
                // get pos
                Vector3 my_pos = hit.collider.transform.GetChild(0).position;

                finish_pos.SetActive(false);
                //jump
                transform.DOJump(my_pos, power_jump, 1, duration).OnComplete(() => on_final());



            }
        }

        public void jumping(Vector3 vec, bool bl = false, Glass glass_sc = null)
        {
            // jump animation
            anim.Play("jump");

            if (bl)
            {
                transform.DOJump(vec, power_jump, 1, duration).OnComplete(() => on_success());
            }
            else
            {
                transform.DOJump(vec, power_jump, 1, duration).OnComplete(() => on_player_fall(glass_sc));
            }

        }

        public void on_player_fall(Glass glass_sc)
        {
            StartCoroutine(lose_panel_wait());
            //inactive timer
            canCountTime = false;

            //reset all glasses material
            for (int i = 0; i < list_glasses.Length; i++)
            {
                list_glasses[i].GetComponent<MeshRenderer>().material = mats[0];
                list_glasses[i].is_active = false;
            }
            SoundManager.Instance.PLaySoundGlassBreak();
            glass_sc.break_glass.SetActive(true);
            glass_sc.GetComponent<MeshRenderer>().enabled = false;
            glass_sc.GetComponent<MeshCollider>().enabled = false;

            // game run

            game_run = false;

            // fall animation
            anim.Play("fall");

            //addforce
            rb.isKinematic = false;
            rb.AddForce(Vector3.down * fall_power, ForceMode.Impulse);

        }

        public void on_final()
        {
            //play confetti
            confetti.Play();
            game_run = false;
            cam_follow.is_active = false;
            anim.Play("happy");

            Quaternion qt1 = Quaternion.Euler(0, -180, 0);

            Vector3 vt1 = qt1.eulerAngles;

            transform.DORotate(vt1, .5f).SetEase(ease);

            //cam_pos_finish

            Quaternion qt = cam_pos_finish.transform.rotation;
            Vector3 vt = qt.eulerAngles;
            sequence = DOTween.Sequence();
            sequence.Append(my_cam.transform.DOMove(cam_pos_finish.position, .6f).SetEase(ease))
                        .Append(my_cam.transform.DORotate(vt, .5f).SetEase(ease))
                        .OnComplete(() => break_all_glasses_finish());
            // win panel 
            StartCoroutine(win_panel_wait());
        }

        public void cam_to_player_pos()
        {
            Quaternion qt = cam_pos_1.transform.rotation;
            Vector3 vt = qt.eulerAngles;
            sequence = DOTween.Sequence();
            sequence.Append(my_cam.transform.DOMove(cam_pos_1.position, 1f).SetEase(ease))
                        .Join(my_cam.transform.DORotate(vt, 1f).SetEase(ease))
                        .OnComplete(() => get_next_step());
        }

        public void on_success()
        {
            get_next_step();
        }


        public void break_all_glasses_finish()
        {
            Glass[] all_glasses = FindObjectsOfType<Glass>();

            for (int i = 0; i < all_glasses.Length; i++)
            {
                all_glasses[i].break_glass.SetActive(true);
                all_glasses[i].GetComponent<MeshRenderer>().enabled = false;
                all_glasses[i].GetComponent<MeshCollider>().enabled = false;
            }

        }

        public void break_all_glasses_and_player_fall_timeOut()
        {
            Glass[] all_glasses = FindObjectsOfType<Glass>();

            //reset all glasses material
            for (int i = 0; i < list_glasses.Length; i++)
            {
                list_glasses[i].GetComponent<MeshRenderer>().material = mats[0];
                list_glasses[i].is_active = false;
            }

            //break all glasses
            for (int i = 0; i < all_glasses.Length; i++)
            {
                all_glasses[i].break_glass.SetActive(true);
                all_glasses[i].GetComponent<MeshRenderer>().enabled = false;
                all_glasses[i].GetComponent<MeshCollider>().enabled = false;
            }

            //animation lose
            anim.Play("fall");

            //addforce to player 
            rb.isKinematic = false;
            rb.AddForce(Vector3.down * fall_power, ForceMode.Impulse);

        }


        IEnumerator win_panel_wait()
        {
            SoundManager.Instance.PlaySoundWin();
            canCountTime = false;
            yield return new WaitForSeconds(4f);
            UIGlassSteppingController.Instance.UIGamePlay.DisplayPanelGameplay(false);
            UIGlassSteppingController.Instance.UIWin.DisplayPanelWin(true);
        }

        IEnumerator lose_panel_wait()
        {
            SoundManager.Instance.PlaySoundLose();
            yield return new WaitForSeconds(4f);
            UIGlassSteppingController.Instance.UIGamePlay.DisplayPanelGameplay(false);
            UIGlassSteppingController.Instance.UILose.DisplayPanelLose(true);
        }
    }
}
