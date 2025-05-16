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

        public Glass[] listGlass;
        public LayerMask glass_layer, finish_layer;
        public List<MeshRenderer> listTrueGlass;
        public Material[] mats;
        public float tm;
        public Camera cam;
        public int currentStep;
        public bool gameRun, activeFinish, showGuid;
        public float powerJump, duration, fallPower;
        public float totalTime, max_time, timer;
        public bool canCountTime = false;
        public Vector3 mypos;

        Rigidbody rb;
        public Animator anim;
        public GameObject finish_pos;

        //camera
        public GameObject playCam, endCam;
        public ParticleSystem confetti;
        Vector3 revivePos;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            rb = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
        }

        void Start()
        {
            revivePos = transform.position;
            mypos = transform.position;
            SoundManager.Instance.PlayBGMusic4();
            UIGlassSteppingController.Instance.UIGamePlay.SetTimeText(totalTime);
        }

        void Update()
        {
            if (!gameRun || !canCountTime)
                return;

            if (timer >= max_time)
            {
                totalTime -= 1f;
                UIGlassSteppingController.Instance.UIGamePlay.SetTimeText(totalTime);
                timer = 0f;
            }
            else
            {
                timer += Time.deltaTime;
            }

            if (totalTime <= 0)
            {
                totalTime = 0f;
                UIGlassSteppingController.Instance.UIGamePlay.SetTimeText(totalTime);
                canCountTime = false;
                StartCoroutine(ShowLose());
            }

            if (Input.GetMouseButtonDown(0))
            {
                //ray cast
                ChooseGlass();
            }
        }

        public void StartGame()
        {
            StartCoroutine(ShowTrueGlass(tm));
        }

        IEnumerator ShowLose()
        {
            //break_all_glasses_and_player_fall_timeOut();
            if (!Manager.Instance.isRevived)
            {
                UIRevive.Instance.DisplayRevivePanel(true);
                yield break;
            }
            SoundManager.Instance.StopMusic();
            SoundManager.Instance.PlaySoundLose();
            yield return new WaitForSeconds(3.5f);
            UIGlassSteppingController.Instance.UIGamePlay.DisplayPanelGameplay(false);
        }

        IEnumerator ShowTrueGlass(float tm)
        {
            currentStep = 0;
            listTrueGlass.Clear();
            // fill all glasses
            for (int i = 0; i < listGlass.Length; i += 2)
            {
                int rdm = Random.Range(0, 2);
                if (rdm == 0)
                {
                    listGlass[i].type = glass_type.true_glass;
                    listGlass[i + 1].type = glass_type.wrong_glass;
                    // add to list true glasses
                    MeshRenderer ms = listGlass[i].GetComponent<MeshRenderer>();
                    listTrueGlass.Add(ms);
                }
                else
                {
                    if (rdm == 1)
                    {
                        listGlass[i].type = glass_type.wrong_glass;
                        listGlass[i + 1].type = glass_type.true_glass;
                        // add to list true glasses
                        MeshRenderer ms = listGlass[i + 1].GetComponent<MeshRenderer>();
                        listTrueGlass.Add(ms);
                    }
                }
            }
            // make hint by animation
            for (int i = 0; i < listTrueGlass.Count; i++)
            {
                listTrueGlass[i].material = mats[2];

                if (i != 0)
                {
                    listTrueGlass[i - 1].material = mats[0];
                }

                yield return new WaitForSeconds(tm);
                if (i == listTrueGlass.Count - 1)
                {
                    listTrueGlass[i].material = mats[0];
                }
            }

            // show all answer and hide them again
            //show
            int hintCount = 0;
            while (hintCount < 2)
            {
                hintCount++;
                for (int i = 0; i < listTrueGlass.Count; i++)
                {
                    listTrueGlass[i].material = mats[2];
                }
                yield return new WaitForSeconds(0.5f);
                //hide
                for (int i = 0; i < listTrueGlass.Count; i++)
                {
                    listTrueGlass[i].material = mats[0];
                }
                yield return new WaitForSeconds(0.5f);
            }

            // run the game
            //game_run = true;

            // camera 
            playCam.SetActive(true);


            // start steps 
            DOVirtual.DelayedCall(1f, delegate
            {
                GetNextStep();
            });
        }

        public void GetNextStep()
        {
            if (!showGuid)
            {
                showGuid = true;
                UIGlassSteppingController.Instance.guid.SetActive(true);
            }
            if (currentStep == listTrueGlass.Count)
            {
                activeFinish = true;
                finish_pos.SetActive(true);

                //reset all glasses material
                for (int i = 0; i < listGlass.Length; i++)
                {
                    listGlass[i].GetComponent<MeshRenderer>().material = mats[0];
                    listGlass[i].is_active = false;
                }
            }
            else
            {
                // idle animation
                anim.Play("idle_glass");
                //reset all glasses material
                for (int i = 0; i < listGlass.Length; i++)
                {
                    listGlass[i].GetComponent<MeshRenderer>().material = mats[0];
                    listGlass[i].is_active = false;
                }
                // change materials of next step
                int step = currentStep * 2;
                int cnt = 0;
                for (int i = step; i < listGlass.Length; i++)
                {
                    listGlass[i].GetComponent<MeshRenderer>().material = mats[1];
                    listGlass[i].is_active = true;
                    cnt++;
                    if (cnt >= 2)
                        break;
                }
                currentStep++;
            }
        }

        public void Revive()
        {
            totalTime += 20f;
            canCountTime = true;
            rb.isKinematic = true;
            transform.rotation = Quaternion.identity;
            transform.position = revivePos;
            gameRun = true;
            anim.Play("idle");
            currentStep--;
            GetNextStep();
        }

        public void ChooseGlass()
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
                        //get jump position
                        Vector3 jump_pos = glass.pos_jumping.transform.position;
                        revivePos = jump_pos;
                        Jumping(jump_pos, true);
                    }
                    else if (glass.type == glass_type.wrong_glass)
                    {
                        //get jump position
                        Vector3 jump_pos = glass.pos_jumping.transform.position;
                        Jumping(jump_pos, false, glass);
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
                transform.DOJump(my_pos, powerJump, 1, duration).OnComplete(() => ReachFinal());
            }
        }

        public void Jumping(Vector3 vec, bool isTrueGlass = false, Glass glass_sc = null)
        {
            // jump animation
            anim.Play("jump");
            if (isTrueGlass)
            {
                transform.DOJump(vec, powerJump, 1, duration).OnComplete(() => GetNextStep());
            }
            else
            {
                transform.DOJump(vec, powerJump, 1, duration).OnComplete(() => PlayerFalling(glass_sc));
            }
        }

        public void PlayerFalling(Glass glass_sc)
        {

            StartCoroutine(ShowLosePanel());
            //inactive timer
            canCountTime = false;

            //reset all glasses material
            for (int i = 0; i < listGlass.Length; i++)
            {
                listGlass[i].GetComponent<MeshRenderer>().material = mats[0];
                listGlass[i].is_active = false;
            }
            SoundManager.Instance.PLaySoundGlassBreak();
            glass_sc.break_glass.SetActive(true);
            glass_sc.GetComponent<MeshRenderer>().enabled = false;
            glass_sc.GetComponent<MeshCollider>().enabled = false;

            // game run

            gameRun = false;

            // fall animation
            anim.Play("fall");

            //addforce
            rb.isKinematic = false;
            rb.AddForce(Vector3.down * fallPower, ForceMode.Impulse);

        }

        public void ReachFinal()
        {
            //play confetti
            confetti.Play();
            gameRun = false;
            anim.Play("happy");
            Quaternion qt1 = Quaternion.Euler(0, -180, 0);
            Vector3 vt1 = qt1.eulerAngles;
            transform.DORotate(vt1, .5f).SetEase(Ease.Linear);

            //cam_pos_finish
            endCam.SetActive(true);
            DOVirtual.DelayedCall(1f, delegate
            {
                BreakAllGlassTrue();
            });

            // win panel 
            StartCoroutine(ShowWinPanel());
        }

        public void BreakAllGlassTrue()
        {
            Glass[] all_glasses = FindObjectsOfType<Glass>();

            for (int i = 0; i < all_glasses.Length; i++)
            {
                if (all_glasses[i].type == glass_type.wrong_glass)
                {
                    all_glasses[i].break_glass.SetActive(true);
                    all_glasses[i].GetComponent<MeshRenderer>().enabled = false;
                    all_glasses[i].GetComponent<MeshCollider>().enabled = false;
                }
            }
        }

        public void break_all_glasses_and_player_fall_timeOut()
        {
            Glass[] all_glasses = FindObjectsOfType<Glass>();

            //reset all glasses material
            for (int i = 0; i < listGlass.Length; i++)
            {
                listGlass[i].GetComponent<MeshRenderer>().material = mats[0];
                listGlass[i].is_active = false;
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
            rb.AddForce(Vector3.down * fallPower, ForceMode.Impulse);

        }


        IEnumerator ShowWinPanel()
        {
            SoundManager.Instance.StopMusic();
            SoundManager.Instance.PlaySoundWin();
            canCountTime = false;
            yield return new WaitForSeconds(4f);
            UIGlassSteppingController.Instance.UIGamePlay.DisplayPanelGameplay(false);
            UIGlassSteppingController.Instance.UIWin.DisplayPanelWin(true);
        }

        IEnumerator ShowLosePanel()
        {
            SoundManager.Instance.StopMusic();
            SoundManager.Instance.PlaySoundLose();
            yield return new WaitForSeconds(4f);
            UIGlassSteppingController.Instance.UIGamePlay.DisplayPanelGameplay(false);
            if (!Manager.Instance.isRevived)
            {
                UIRevive.Instance.DisplayRevivePanel(true);
            }
            else
            {
                UIGlassSteppingController.Instance.UILose.DisplayPanelLose(true);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.CompareTag("Obstacle"))
            {
                SoundManager.Instance.PlaySoundMaleHited();
                anim.Play("FlyingBackDeath");
            }
        }
    }
}
