using System.Collections;
using UnityEngine;

public class aiplayer : MonoBehaviour
{
    PlayerController pc;
    enemCtr ec;
    public float speed;
    public bool firsttime;
    float a;
    bool die,onetime,win;
    public bool femal;

    [Header("Animation")]
    Animator animator;
    float velocity = 0f;
    public float acceleration = 0.2f;
    int veclocityHash;
    bool canRandom = false;
    string[] runPoses = { "runfe", "rundynamicpose", "rundancepose" };
    int randomePoseIndex;


    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(2.8f, 3.4f);
        ec = FindObjectOfType<enemCtr>();
        pc = FindObjectOfType<PlayerController>();
        int tt = Random.Range(1, 6);
        GetComponent<Animator>().Play("idle"+tt.ToString());
        animator = GetComponent<Animator>();
        veclocityHash = Animator.StringToHash("velocity");
    }

    // Update is called once per frame
    void Update()
    {
        if(pc.GmRun && !die && !win)
        {
            if(!ec.animcor)
            {
                onetime = false;
                if(!firsttime)
                {
                    firsttime = true;
                    a = Random.Range(1, 1.3f);
                    SetRandomPoseWhileRunning(false);
                }
                else
                {
                    SetRandomPoseWhileRunning(false);
                }
                transform.Translate(Vector3.forward * speed * Time.deltaTime);
                GetComponent<Animator>().speed = a;
            }
            else
            {
                speed = Random.Range(3f, 4f);
                if(!onetime)
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
            if (velocity > 1) velocity = 1;
            animator.SetFloat(veclocityHash, velocity);
        }
        else
        {
            if (canRandom)
            {
                randomePoseIndex = Random.Range(0, runPoses.Length);
                canRandom = false;
            }
            velocity -= Time.deltaTime * acceleration * 2;
            if (velocity <= 0)
            {
                animator.Play(runPoses[randomePoseIndex]);
                velocity = 0;
            }
            animator.SetFloat(veclocityHash, velocity);
        }
    }

    IEnumerator stayordie()
    {
        onetime = true;
        int b = Random.Range(1, 4);
        if(b==2)
        {
            die = true;
            GetComponent<HighlightPlus.HighlightEffect>().highlighted = true;
            GetComponent<Animator>().speed =Random.Range(1,2);
        }
        else
        {
            GetComponent<Animator>().speed = 0;
        }

        yield return new WaitForSeconds(Random.Range(0.5f,0.7f));
        GetComponent<Animator>().speed = 0;
        yield return new WaitForSeconds(Random.Range(0.8f,1.5f));
        if(die)
        {

            GetComponent<BoxCollider>().isTrigger = true;
            int t = Random.Range(1, 3);
            SoundManager.Instance.PlaySoundGunShooting();
            yield return new WaitForSeconds(0.1f);
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
            int bb = Random.Range(1, 5);
            GetComponent<HighlightPlus.HighlightEffect>().highlighted = false;
            GetComponent<Animator>().Play("die" + bb.ToString());
            GetComponent<Animator>().speed = 1;
            Destroy(gameObject, 7f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "win")
        {
            win = true;
            transform.eulerAngles = new Vector3(0, 180, 0);
            GetComponent<Animator>().Play("idle2");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }


}
