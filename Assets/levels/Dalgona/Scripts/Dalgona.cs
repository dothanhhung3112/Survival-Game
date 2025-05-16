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
        public Transform[] dalgonaPartPos;
        public GameObject dalgonaCenter;
        public GameObject[] dalgonaParts, dalgonaBreakParts, dalgonaCenterBreakParts;
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
                AnimateNeedle("needle_zigzag_simple");
            }

            if (Input.GetMouseButtonUp(0))
            {
                // pause
                SoundManager.Instance.StopSound();
                dot_path.DOPause();
                effect.Stop();
                AnimateNeedle("needle_idle");

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
                CheckingClickTimeOut();
            }

            // check reached 
            CheckReachedFinish();
        }

        public void CheckReachedFinish()
        {
            if (Vector3.Distance(needle.position, dalgonaPartPos[actual_part].position) <= .2f)
            {
                SoundManager.Instance.PlaySoundCandyBreak();
                if (actual_part >= dalgonaPartPos.Length - 1)
                {
                    active = false;
                    dalgonaParts[actual_part].SetActive(false);
                    dalgonaBreakParts[actual_part].SetActive(true);

                    //animate to idle
                    AnimateNeedle("needle_idle");

                    //win panel
                    StartCoroutine(ShowWin());
                }
                else
                {
                    dalgonaParts[actual_part].SetActive(false);
                    dalgonaBreakParts[actual_part].SetActive(true);
                    actual_part++;
                }
            }
        }

        void CheckingClickTimeOut()
        {
            if (timer >= max_time)
            {
                active = false;
                // pause
                dot_path.DOPause();

                //animate to idle
                AnimateNeedle("needle_idle");

                //lose panel
                StartCoroutine(ShowLose());

            }
            else
            {
                timer += Time.deltaTime;

                if (timer >= max_time - 1.5f)
                {
                    //change animation 
                    AnimateNeedle("needle_zigzag_timeout");
                    failure_vibrate();
                }
                else
                {
                    //print("success vibrate");
                    SuccessVibrate();
                }
            }
        }

        void AnimateNeedle(string name)
        {
            if (name != actual_anim)
            {
                anim.Play(name);
                actual_anim = name;
            }
        }

        public void ReviveDalgona()
        {
            active = true;
            dalgonaCenter.SetActive(true);
            for (int i = 0; i < dalgonaCenterBreakParts.Length; i++)
            {
                dalgonaCenterBreakParts[i].SetActive(false);
            }
        }

        IEnumerator ShowLose()
        {
            if (DalgonaController.Instance.isLose || DalgonaController.Instance.isWin) yield break;
            DalgonaController.Instance.isLose = true;
            DalgonaController.Instance.canCountTime = false;
            UIDalgonaController.Instance.UIGamePlay.DisplayPanelGameplay(false);
            if (!Manager.Instance.isRevived)
            {
                UIRevive.Instance.DisplayRevivePanel(true);
                yield break;
            }
            DalgonaController.Instance.StatingEndCard();
            //hide dalgona center
            dalgonaCenter.SetActive(false);
            for (int i = 0; i < dalgonaCenterBreakParts.Length; i++)
            {
                dalgonaCenterBreakParts[i].SetActive(true);
            }
            SoundManager.Instance.StopMusic();
            SoundManager.Instance.PlaySoundLose();
            yield return new WaitForSeconds(3f);
            UIDalgonaController.Instance.UILose.DisplayPanelLose(true);
        }

        IEnumerator ShowWin()
        {
           
            if (DalgonaController.Instance.isLose || DalgonaController.Instance.isWin) yield break;
            DalgonaController.Instance.isWin = true;
            DalgonaController.Instance.canCountTime = false;
            UIDalgonaController.Instance.UIGamePlay.DisplayPanelGameplay(false);
            DalgonaController.Instance.Winning();
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
