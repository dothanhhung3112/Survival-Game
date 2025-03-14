using System.Collections;
using UnityEngine;
using DG.Tweening;
using Hung.UI;

namespace Hung.Gameplay.Dalgona
{
    public class Dalgona : MonoBehaviour
    {
        public DOTweenPath dot_path;
        public Animator anim;
        public Transform[] list_dalgona_part_pos;
        public GameObject dalgona_center;
        public GameObject[] dalgona_parts, dalgona_break_parts, dalgona_center_break_parts;
        public int actual_part;
        public Transform needle;
        ParticleSystem effect;
        public bool active;

        public float max_time, timer;
        public string actual_anim;

        //vibrate setting
        public float vibrate_timer_success, vibrate_max_times_success, vibrate_timer_failure, vibrate_max_times_failure;
        void Start()
        {
            actual_anim = "needle_idle";
            effect = Instantiate(DalgonaController.Instance.effectDalgona, needle.transform);
            effect.transform.localScale = Vector3.one * 70f;
        }

        float elapsedTime = 1f;

        void Update()
        {
            if (!active) return;

            if (Input.GetMouseButtonDown(0))
            {
                // play
                elapsedTime = 1f;
                dot_path.DOPlay();
                effect.Play();
                animate_needle("needle_zigzag_simple");
            }

            if (Input.GetMouseButtonUp(0))
            {
                // pause
                SoundManager.Instance.StopSound();
                dot_path.DOPause();
                effect.Stop();
                animate_needle("needle_idle");

                //reset timer
                timer = 0f;

                //reset vibrate timer
                vibrate_timer_success = 0f;
                vibrate_timer_failure = 0f;

            }

            if (Input.GetMouseButton(0))
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime > 1f)
                {
                    SoundManager.Instance.PlaySoundNeedleMove();
                    elapsedTime = 0;
                }
                // check click timeout
                check_click_timeout();
            }

            // check reached 
            check_reached_finish_part();
        }

        public void check_reached_finish_part()
        {
            if (Vector3.Distance(needle.position, list_dalgona_part_pos[actual_part].position) <= .2f)
            {
                SoundManager.Instance.PlaySoundCandyBreak();
                if (actual_part >= list_dalgona_part_pos.Length - 1)
                {
                    active = false;
                    dalgona_parts[actual_part].SetActive(false);
                    dalgona_break_parts[actual_part].SetActive(true);

                    //animate to idle
                    animate_needle("needle_idle");

                    //win panel
                    StartCoroutine(ShowWin());
                }
                else
                {
                    dalgona_parts[actual_part].SetActive(false);
                    dalgona_break_parts[actual_part].SetActive(true);
                    actual_part++;
                }
            }
        }

        void check_click_timeout()
        {
            if (timer >= max_time)
            {
                active = false;
                print("timeout");

                // pause
                dot_path.DOPause();

                //animate to idle
                animate_needle("needle_idle");

                //lose panel
                StartCoroutine(show_lose());

            }
            else
            {
                timer += Time.deltaTime;

                if (timer >= max_time - 1.5f)
                {
                    //change animation 
                    animate_needle("needle_zigzag_timeout");
                    failure_vibrate();
                }
                else
                {
                    //print("success vibrate");
                    SuccessVibrate();
                }
            }
        }

        void animate_needle(string name)
        {
            if (name != actual_anim)
            {
                anim.Play(name);
                actual_anim = name;
            }
        }

        IEnumerator show_lose()
        {
            DalgonaController.Instance.canCountTime = false;
            UIDalgonaController.Instance.UIGamePlay.DisplayPanelGameplay(false);
            DalgonaCam cam_script = FindObjectOfType<DalgonaCam>();
            cam_script.lose_move();
            //hide dalgona center
            dalgona_center.SetActive(false);
            for (int i = 0; i < dalgona_center_break_parts.Length; i++)
            {
                dalgona_center_break_parts[i].SetActive(true);
            }
            if (actual_part < list_dalgona_part_pos.Length - 1)
            {
                dalgona_parts[actual_part].SetActive(false);
                dalgona_break_parts[actual_part].SetActive(true);
            }
            SoundManager.Instance.StopMusic();
            SoundManager.Instance.PlaySoundLose();
            yield return new WaitForSeconds(3f);
            UIDalgonaController.Instance.UILose.DisplayPanelLose(true);
        }

        IEnumerator ShowWin()
        {
            DalgonaController.Instance.canCountTime = false;
            UIDalgonaController.Instance.UIGamePlay.DisplayPanelGameplay(false);
            DalgonaCam cam_script = FindObjectOfType<DalgonaCam>();
            cam_script.win_move();
            SoundManager.Instance.StopMusic();
            SoundManager.Instance.PlaySoundWin();
            yield return new WaitForSeconds(3f);
            UIDalgonaController.Instance.UIWin.DisplayPanelWin(true);
        }

        void SuccessVibrate()
        {
            if (vibrate_timer_success >= vibrate_max_times_success)
            {
                // success vibrate
                vibrate_timer_success = 0f;
            }
            else
            {
                vibrate_timer_success += Time.deltaTime;
            }
        }

        void failure_vibrate()
        {
            if (vibrate_timer_failure >= vibrate_max_times_failure)
            {
                // success vibrate
                vibrate_timer_failure = 0f;
            }
            else
            {
                vibrate_timer_failure += Time.deltaTime;
            }
        }
    }
}
