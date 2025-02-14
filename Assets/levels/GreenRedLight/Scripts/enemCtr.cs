using System.Collections;
using UnityEngine;
using Hung.UI;

namespace Hung.Gameplay.GreenRedLight
{
    public class enemCtr : MonoBehaviour
    {
        PlayerController pc;
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

        // Update is called once per frame
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

                    if (transform.eulerAngles.y > 181f || transform.eulerAngles.y == 0)
                    {
                        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y - 75 * i * Time.deltaTime, 0);
                    }
                    if (!animcor && !pc.die && !pc.win)
                    {
                        StartCoroutine(singagain());
                    }
                }

                if (transform.eulerAngles.y < 350f && startturn)
                {
                    transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 75 * i * Time.deltaTime, 0);
                }

                if (transform.eulerAngles.y >= 349f && startturn)
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

        IEnumerator singagain()
        {
            animcor = true;
            yield return new WaitForSeconds(1.5f);
            SoundManager.Instance.PlaySoundEnemySearching();
            yield return new WaitForSeconds(3f);
            startturn = true;

        }
    }
}
