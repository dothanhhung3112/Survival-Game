using System.Collections;
using UnityEngine;
using Hung.UI;
using Hung.Tools;

namespace Hung.Gameplay.GreenRedLight
{
    public class enemCtr : MonoBehaviour
    {
        PlayerController pc;
        [SerializeField] Transform[] startBulletPos;
        [SerializeField] Transform[] endBulletPos;
        [SerializeField] Bullet bulletPrefab;
        [SerializeField] Transform headDoll;
        public float mytimer;
        public bool onetime, firsttime, endcount, startturn, animcor;
        public int i;
        public float a = 1;
        float timestage;
        
        void Start()
        {
            pc = FindObjectOfType<PlayerController>();
            timestage = mytimer;
        }

        void Update()
        {
            if (pc.GmRun && !pc.die)
            {
                if (!firsttime)
                {
                    if (i < 5)
                    {
                        i++;
                    }

                    firsttime = true;
                    SoundManager.Instance.PlaySoundEnemy(a);
                    if (a < 2)
                    {
                        a = 1 + (0.2f * i);
                    }
                }
                if (mytimer >= 0 && !endcount)
                {
                    UIGreenRedLightController.Instance.UIGamePlay.SetRedImageFillAmount(1 / timestage * Time.deltaTime);
                    mytimer -= Time.deltaTime;
                }

                if (mytimer < 0 && !startturn)
                {
                    endcount = true;

                    if (headDoll.eulerAngles.y > 181f || headDoll.eulerAngles.y == 0)
                    {
                        headDoll.eulerAngles = new Vector3(0, headDoll.eulerAngles.y - 75 * i * Time.deltaTime * 5f, 0);
                    }
                    if (!animcor && !pc.die && !pc.win)
                    {
                        StartCoroutine(SingAgain());
                    }
                }

                if (headDoll.eulerAngles.y < 350f && startturn)
                {
                    headDoll.eulerAngles = new Vector3(0, headDoll.eulerAngles.y + 75 * i * Time.deltaTime * 5f, 0);
                }

                if (headDoll.eulerAngles.y >= 349f && startturn)
                {
                    timestage = mytimer = 4.7f / a;
                    startturn = false;
                    firsttime = false;
                    endcount = false;
                    animcor = false;
                    UIGreenRedLightController.Instance.UIGamePlay.TurnOnGreenImage();
                }
            }
        }

        public void SpawnBullet(Transform bot)
        {
            for(int i = 0;i < 2; i++)
            {
                int randomStartPos = Random.Range(0, startBulletPos.Length);
                Bullet bullet = Instantiate(bulletPrefab, startBulletPos[randomStartPos]);
                bullet.Launch(Random.Range(100,120), bot, bot.position);
            }
        }

        IEnumerator SingAgain()
        {
            animcor = true;
            yield return new WaitForSeconds(1.5f);
            SoundManager.Instance.PlaySoundEnemySearching();
            yield return new WaitForSeconds(3f);
            startturn = true;

        }
    }
}
