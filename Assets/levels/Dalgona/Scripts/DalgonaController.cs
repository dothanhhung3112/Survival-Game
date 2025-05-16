using DG.Tweening;
using Hung.UI;
using System.Collections;
using UnityEngine;

namespace Hung.Gameplay.Dalgona
{
    public class DalgonaController : MonoBehaviour
    {
        public static DalgonaController Instance;
        public GameObject camLose, camWin, camTable;
        public ParticleSystem effectDalgona;
        public Dalgona dalgonaChosen;
        public BoxDalgona boxDalgona;
        public Animator animPlayer, animEnemy;
        public bool canCountTime = false;
        public bool active,isWin,isLose;
        public float timeLeft;
        public LayerMask box_layer;

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
            UIDalgonaController.Instance.UIGamePlay.SetTimeText(timeLeft);
        }

        void Update()
        {
            if (canCountTime)
            {
                timeLeft -= Time.deltaTime;
                int a = (int)timeLeft;
                if (a >= 0)
                {
                    UIDalgonaController.Instance.UIGamePlay.SetTimeText(a);
                }

                if (timeLeft <= 0)
                {
                    timeLeft = 0f;
                    UIDalgonaController.Instance.UIGamePlay.SetTimeText(a);
                    canCountTime = false;
                    dalgonaChosen.active = false;
                    StartCoroutine(ShowLosePanel());
                }
            }

            if (!active) return;
            if (Input.GetMouseButtonDown(0))
            {
                ChooseBox();
            }
        }

        void ChooseBox()
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

        public void Revive()
        {
            isLose = false;
            timeLeft += 10f;
            dalgonaChosen.active = true;
            canCountTime = true;
            UIDalgonaController.Instance.UIGamePlay.DisplayPanelGameplay(true);
        }

        public void StartGame()
        {
            dalgonaChosen.active = true;
        }

        public void ShootingPlayer()
        {
            animEnemy.Play("Shooting");
            SoundManager.Instance.PlaySoundGunShooting();
            DOVirtual.DelayedCall(0.2f, delegate
            {
                animPlayer.Play("Die");
                GameObject blood = ObjectPooler.instance.SetObject("bloodEffect", animPlayer.transform.position + new Vector3(0, 55, 0));
                animPlayer.transform.localPosition += new Vector3(0, 8, 0);
                blood.transform.localScale *= 50f;
                SoundManager.Instance.PlaySoundMaleHited();
            });
        }

        public void StatingEndCard()
        {        
            camLose.SetActive(true);
            DOVirtual.DelayedCall(2f, delegate
            {
                ShootingPlayer();
            });
        }

        public void Winning()
        {
            animPlayer.transform.DORotate(new Vector3(0, -180, 0),1f);
            camWin.SetActive(true);
            DOVirtual.DelayedCall(2f, delegate
            {
                boxDalgona.camBox.SetActive(false);
                animPlayer.Play("happy");
            });
        }

        IEnumerator ShowLosePanel()
        {
            if (isWin) yield break;
            DalgonaController.Instance.canCountTime = false;
            if (!Manager.Instance.isRevived)
            {
                UIRevive.Instance.DisplayRevivePanel(true);
                yield break;
            }
            StatingEndCard();
            SoundManager.Instance.StopMusic();
            yield return new WaitForSeconds(3.5f);
            UIDalgonaController.Instance.UIGamePlay.DisplayPanelGameplay(false);
            UIDalgonaController.Instance.UILose.DisplayPanelLose(true);
        }
    }
}
