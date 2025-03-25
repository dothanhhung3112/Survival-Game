using System.Collections;
using UnityEngine;
using Hung.UI;
using DG.Tweening;

namespace Hung.Gameplay.GreenRedLight {
    public class aiplayer : MonoBehaviour
    {
        PlayerController pc;
        enemCtr ec;
        public float speed;
        public bool firsttime;
        float a;
        bool die, onetime, win;
        public bool femal;

        [Header("Animation")]
        Animator animator;
        float velocity = 0f;
        public float acceleration = 0.2f;
        int veclocityHash;
        int blendHash;
        bool canRandom = true;
        float[] runPoses = { 0, 0.5f, 1 };
        int randomePoseIndex;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            ec = FindObjectOfType<enemCtr>();
            pc = FindObjectOfType<PlayerController>();
        }

        void Start()
        {
            speed = Random.Range(2.8f, 3.4f);
            int tt = Random.Range(1, 6);
            animator.Play("idle" + tt.ToString());
            veclocityHash = Animator.StringToHash("velocity");
            blendHash = Animator.StringToHash("Blend");
        }

        void Update() 
        {

            if (pc.GmRun && !die && !win && !pc.die)
            {
                if (!ec.animcor)
                {
                    onetime = false;
                    if (!firsttime)
                    {
                        firsttime = true;
                        a = Random.Range(1, 1.3f);
                        SetRandomPoseWhileRunning(false);
                        animator.Play("runPose");
                    }
                    else
                    {
                        SetRandomPoseWhileRunning(false);
                    }
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);
                    animator.speed = a;
                }
                else
                {
                    speed = Random.Range(3f, 4f);
                    if (!onetime)
                    {
                        StartCoroutine(stayordie());
                    }
                    if (!die)
                    {
                        SetRandomPoseWhileRunning(true);
                    }
                }
            }
            if (UIGreenRedLightController.Instance.time <= 0 && !win && !die)
            {
                die = true;
                StartCoroutine(stayordie());
            }
        }

        void SetRandomPoseWhileRunning(bool enable)
        {
            if (enable)
            {
                canRandom = true;
                velocity += Time.deltaTime * acceleration;
                if (velocity > 1)
                {
                    velocity = 1;
                }
                animator.SetFloat(veclocityHash, velocity);
            }
            else
            {

                velocity -= Time.deltaTime * acceleration;
                if (velocity <= 0)
                {
                    velocity = 0;
                    if (canRandom)
                    {
                        randomePoseIndex = Random.Range(0, runPoses.Length);
                        canRandom = false;
                        animator.SetFloat(blendHash, runPoses[randomePoseIndex]);
                    }
                }
                animator.SetFloat(veclocityHash, velocity);
            }
        }

        IEnumerator stayordie()
        {
            onetime = true;
            int b = Random.Range(1, 4);
            if (b == 2)
            {
                die = true;
                GetComponent<HighlightPlus.HighlightEffect>().highlighted = true;
                animator.speed = Random.Range(1, 2);
            }
            else
            {
                animator.speed = 0;
            }

            yield return new WaitForSeconds(Random.Range(0.5f, 0.7f));
            animator.speed = 0;

            if (pc.die) yield break;

            if (pc.die) yield return new WaitForSeconds(Random.Range(4f, 6f));
            else yield return new WaitForSeconds(Random.Range(0.8f, 1.5f));
            if (die)
            {
                GetComponent<BoxCollider>().isTrigger = true;
                int t = Random.Range(1, 3);
                ec.SpawnBullet(transform);
                yield return new WaitForSeconds(0.4f);
                SoundManager.Instance.PlaySoundGunShooting();
                if (femal)
                {
                    SoundManager.Instance.PlaySoundFemaleHited();
                }
                else
                {
                    SoundManager.Instance.PlaySoundMaleHited();
                }
                GameObject gm = ObjectPooler.instance.SetObject("bloodEffect", transform.position);
                gm.transform.position = transform.position;
                int bb = Random.Range(2, 5);
                GetComponent<HighlightPlus.HighlightEffect>().highlighted = false;
                animator.Play("die" + bb.ToString());
                transform.position += new Vector3(0, 0.06f, 0);
                animator.speed = 1;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "win")
            {
                DOVirtual.DelayedCall(0.5f, delegate
                {
                    win = true;
                    transform.eulerAngles = new Vector3(0, 180, 0);
                    animator.Play("Win");
                });
            }
        }
    }
}
