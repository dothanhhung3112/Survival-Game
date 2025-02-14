using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hung.Gameplay.TugOfWar
{
    public class Level_3_Conroller : MonoBehaviour
    {
        public float rope_back_speed, rope_forward_speed;
        public float max_dist_rope_player, max_dist_rope_enemy;

        public bool can_pull, game_run;

        public List<GameObject> players, enemies;
        public Animator knife;
        public GameObject panel_arrow, panel_win, panel_lose;
        public GameObject rope;
        public ParticleSystem[] confetti;
        // Start is called before the first frame update
        float elapsedTime = 5f;

        // Update is called once per frame
        void Update()
        {
            if (!game_run)
                return;

            elapsedTime += Time.deltaTime;
            if (elapsedTime > 5f)
            {
                //SoundManager.instance.Play("tug");
                elapsedTime = 0;
            }

            if (can_pull)
            {
                float inc = rope_back_speed * Time.deltaTime;

                Vector3 tmp = transform.position;
                tmp.z -= inc;
                if (tmp.z <= max_dist_rope_player)
                {
                    game_run = false;


                    //shoot enemies
                    StartCoroutine(shoot(enemies));

                    //win panel
                    tmp.z = max_dist_rope_player;
                    transform.position = tmp;

                    print("win");
                    return;
                }
                transform.position = tmp;
            }
            else
            {
                float inc = rope_forward_speed * Time.deltaTime;

                Vector3 tmp = transform.position;
                tmp.z += inc;
                if (tmp.z >= max_dist_rope_enemy)
                {
                    game_run = false;

                    //shoot players
                    StartCoroutine(shoot(players));

                    //lose panel
                    tmp.z = max_dist_rope_enemy;
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
                game_run = false;

                //win panel
                print("win");

                //shoot enemies
                StartCoroutine(shoot(enemies));
            }
        }

        public void check_lose()
        {
            if (players.Count <= 6)
            {
                game_run = false;

                //lose panel
                print("lose");

                //shoot players

                StartCoroutine(shoot(players));
            }
        }

        IEnumerator shoot(List<GameObject> lst)
        {

            knife.Play("knife_down");
            yield return new WaitForSeconds(.1f);
            rope.SetActive(false);
            yield return new WaitForSeconds(.5f);

            panel_arrow.SetActive(false);

            if (lst == players)
            {
                for (int i = 0; i < lst.Count; i++)
                {

                    Vector3 shoot = new Vector3(0f, Random.Range(3, 6), 3f);

                    lst[i].GetComponent<Level_3_Character>().canFall = true;

                    lst[i].GetComponent<Rigidbody>().isKinematic = false;
                    lst[i].GetComponent<Rigidbody>().AddForce(shoot * 9, ForceMode.Impulse);

                }
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].GetComponent<Level_3_Character>().anim.Play("happy");
                }
            }
            else if (lst == enemies)
            {
                for (int i = 0; i < lst.Count; i++)
                {

                    Vector3 shoot = new Vector3(0f, Random.Range(3, 6), -3f);

                    if (lst[i] != null)
                    {
                        lst[i].GetComponent<Level_3_Character>().canFall = true;
                        lst[i].GetComponent<Rigidbody>().isKinematic = false;
                        lst[i].GetComponent<Rigidbody>().AddForce(shoot * 9, ForceMode.Impulse);
                    }

                }

                for (int i = 0; i < players.Count; i++)
                {
                    players[i].GetComponent<Level_3_Character>().anim.Play("happy");
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
            for (int i = 0; i < confetti.Length; i++)
            {
                confetti[i].Play();
            }
            //SoundManager.instance.Play("win");
            yield return new WaitForSeconds(4f);
            panel_arrow.SetActive(false);

            //BridgeController.instance.rewardedCountOnPlay++;
            //BridgeController.instance.ShowBannerCollapsible();
            //if (BridgeController.instance.rewardedCountOnPlay >= 3)
            //{
            //    if (BridgeController.instance.IsRewardReady())
            //    {
            //        BridgeController.instance.rewardedCountOnPlay = 0;
            //    }
            //    panel_win.SetActive(true);

            //    BridgeController.instance.ShowRewarded("win_level_3", null, null);
            //}
            //else
            //{
            //    UnityEvent e = new UnityEvent();
            //    e.AddListener(() =>
            //    {
            //        // luồng game sau khi tắt quảng cáo
            //        panel_win.SetActive(true);
            //    });
            //    BridgeController.instance.ShowInterstitial("win_level_3", e);
            //}
        }

        IEnumerator wait_lose()
        {
            //SoundManager.instance.Play("lose");
            yield return new WaitForSeconds(4f);
            panel_arrow.SetActive(false);

            //BridgeController.instance.rewardedCountOnPlay++;
            //BridgeController.instance.ShowBannerCollapsible();
            //if (BridgeController.instance.rewardedCountOnPlay >= 3)
            //{
            //    if (BridgeController.instance.IsRewardReady())
            //    {
            //        BridgeController.instance.rewardedCountOnPlay = 0;
            //    }
            //    panel_lose.SetActive(true);

            //    BridgeController.instance.ShowRewarded("lose_level_3", null, null);

            //}
            //else
            //{
            //    UnityEvent e = new UnityEvent();
            //    e.AddListener(() =>
            //    {
            //        // luồng game sau khi tắt quảng cáo
            //        panel_lose.SetActive(true);
            //    });
            //    BridgeController.instance.ShowInterstitial("lose_level_3", e);

            //}
        }
    }
}
