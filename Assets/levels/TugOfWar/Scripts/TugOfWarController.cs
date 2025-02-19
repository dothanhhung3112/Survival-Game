using Hung.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hung.Gameplay.TugOfWar
{
    public class TugOfWarController : MonoBehaviour
    {
        public static TugOfWarController Instance;
        public float ropeBackSpeed, ropeForwardSpeed;
        public List<GameObject> players, enemies;
        [HideInInspector] public bool canPull, runnedGame;
        [SerializeField] float maxDisPlayer, maxDistanceEnemy;
        [SerializeField] Animator knifeAnim;
        [SerializeField] GameObject rope;
        [SerializeField] ParticleSystem confetti;
        float elapsedTime = 5f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        void Update()
        {
            if (!runnedGame)
                return;

            elapsedTime += Time.deltaTime;
            if (elapsedTime > 5f)
            {
                SoundManager.Instance.PlaySoundTug();
                elapsedTime = 0;
            }

            if (canPull)
            {
                float inc = ropeBackSpeed * Time.deltaTime;
                Vector3 tmp = transform.position;
                tmp.z -= inc;
                if (tmp.z <= maxDisPlayer)
                {
                    runnedGame = false;


                    //shoot enemies
                    StartCoroutine(shoot(enemies));

                    //win panel
                    tmp.z = maxDisPlayer;
                    transform.position = tmp;

                    print("win");
                    return;
                }
                transform.position = tmp;
            }
            else
            {
                float inc = ropeForwardSpeed * Time.deltaTime;

                Vector3 tmp = transform.position;
                tmp.z += inc;
                if (tmp.z >= maxDistanceEnemy)
                {
                    runnedGame = false;

                    //shoot players
                    StartCoroutine(shoot(players));

                    //lose panel
                    tmp.z = maxDistanceEnemy;
                    transform.position = tmp;

                    print("lose");
                    return;
                }
                transform.position = tmp;
            }
        }

        public void check_win()
        {
            if (enemies.Count <= 6)
            {
                runnedGame = false;
                StartCoroutine(shoot(enemies));
            }
        }

        public void check_lose()
        {
            if (players.Count <= 6)
            {
                runnedGame = false;
                StartCoroutine(shoot(players));
            }
        }

        IEnumerator shoot(List<GameObject> lst)
        {
            knifeAnim.Play("knife_down");
            yield return new WaitForSeconds(.1f);
            rope.SetActive(false);
            yield return new WaitForSeconds(.5f);

            UITugOfWarController.Instance.UIGamePlay.DisplayPanelGameplay(false);

            if (lst == players)
            {
                for (int i = 0; i < lst.Count; i++)
                {

                    Vector3 shoot = new Vector3(0f, Random.Range(3, 6), 3f);

                    lst[i].GetComponent<TugOfWarCharacter>().canFall = true;

                    lst[i].GetComponent<Rigidbody>().isKinematic = false;
                    lst[i].GetComponent<Rigidbody>().AddForce(shoot * 9, ForceMode.Impulse);

                }
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].GetComponent<TugOfWarCharacter>().anim.Play("happy");
                }
            }
            else if (lst == enemies)
            {
                for (int i = 0; i < lst.Count; i++)
                {

                    Vector3 shoot = new Vector3(0f, Random.Range(3, 6), -3f);

                    if (lst[i] != null)
                    {
                        lst[i].GetComponent<TugOfWarCharacter>().canFall = true;
                        lst[i].GetComponent<Rigidbody>().isKinematic = false;
                        lst[i].GetComponent<Rigidbody>().AddForce(shoot * 9, ForceMode.Impulse);
                    }
                }

                for (int i = 0; i < players.Count; i++)
                {
                    players[i].GetComponent<TugOfWarCharacter>().anim.Play("happy");
                }
            }


            if (lst == players)
            {
                StartCoroutine(wait_lose());
            }
            else if (lst == enemies)
            {
                StartCoroutine(wait_win());
            }
        }

        IEnumerator wait_win()
        {
            confetti.Play();
            SoundManager.Instance.PlaySoundWin();
            yield return new WaitForSeconds(4f);
            UITugOfWarController.Instance.UIGamePlay.DisplayPanelGameplay(false);
            UITugOfWarController.Instance.UIWin.DisplayPanelWin(true);
        }

        IEnumerator wait_lose()
        {
            SoundManager.Instance.PlaySoundLose();
            yield return new WaitForSeconds(4f);
            UITugOfWarController.Instance.UIGamePlay.DisplayPanelGameplay(false);
            UITugOfWarController.Instance.UILose.DisplayPanelLose(true);
        }
    }
}
