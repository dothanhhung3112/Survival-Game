using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SquidGamePlayer : MonoBehaviour
{
    public float speed, speedForward;
    public bool gamerun, isWin, stopfolow;
    public Image powerbar;
    public Transform boss, campos, look;
    bool die, kick, win;
    Vector3 direction, pressPos, presspos, actualpos;
    Rigidbody rb;
    float healthMax = 1;
    float playerHealth = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && !gamerun)
        {
            gamerun = true;
            GetComponent<Animator>().Play("run");
            GetComponent<Animator>().speed = 1.2f;
            presspos = Input.mousePosition;
            //FindObjectOfType<UiManager>().startpanel.SetActive(false);
        }

        if (gamerun && !isWin)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speedForward * Time.deltaTime);
        }

        if (Input.GetMouseButton(0))
        {
            actualpos = Input.mousePosition;

            float ss = actualpos.x - presspos.x;
            ss = Mathf.Clamp(ss, -1, 1);
            float xdiff = (actualpos.x - presspos.x) * Time.deltaTime * speed;
            Vector3 tmp = transform.position;
            tmp.x += xdiff;
            tmp.x = Mathf.Clamp(tmp.x, -4.5f, 3f);
            transform.position = Vector3.Lerp(transform.position, tmp, speed * Time.deltaTime);
            presspos = actualpos;
        }

        if (isWin)
        {
            if (Vector3.Distance(transform.position, boss.position) >= 1.2f)
            {
                GetComponent<Animator>().Play("run");
                transform.LookAt(boss);
                transform.position = Vector3.MoveTowards(transform.position, boss.position, 7 * Time.deltaTime);
            }
            else
            {
                if (!stopfolow)
                {
                    //fightpanel.SetActive(true);
                    GetComponent<Animator>().Play("idle1");
                    //SoundManager.instance.Play("punch");
                    FindObjectOfType<SquidEnemy>().canFight = true;
                }
                stopfolow = true;
                transform.LookAt(boss);
                Vector3 v = boss.position;
                Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, campos.position, 7 * Time.deltaTime);
                Camera.main.transform.LookAt(look);
            }
        }

        //if (boss.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().fillAmount <= 0 && !FindObjectOfType<SquidEnemy>().die && isWin)
        //{
        //    FindObjectOfType<SquidEnemy>().die = true;
        //    FindObjectOfType<SquidEnemy>().fight = false;
        //    FindObjectOfType<SquidEnemy>().GetComponent<Animator>().Play("die1");
        //    kick = true;
        //    win = true;
        //    StartCoroutine(winplayer());
        //}

        //if (transform.GetChild(2).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().fillAmount <= 0 && !die && isWin)
        //{
        //    FindObjectOfType<SquidEnemy>().win = true;
        //    FindObjectOfType<SquidEnemy>().fight = false;
        //    FindObjectOfType<SquidEnemy>().GetComponent<Animator>().Play("win");
        //    FindObjectOfType<SquidEnemy>().GetComponent<Animator>().speed = 1;
        //    kick = true;
        //    die = true;
        //    StartCoroutine(dieplayer());
        //}
    }

    public void kicking()
    {
        StartCoroutine(keppkicking());
    }

    IEnumerator keppkicking()
    {
        while (!kick && !die && !win)
        {
            kick = true;
            int bb = Random.Range(1, 4);
            GetComponent<Animator>().Play("kick" + bb.ToString());
            GetComponent<Animator>().speed = 2;
            boss.GetChild(1).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().fillAmount -= 0.085f;
            yield return new WaitForSeconds(0.6f);
            kick = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "food")
        {
            //SoundManager.instance.Play("eatmeat");
            Destroy(collision.gameObject);
            IncreaseHealth();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "win")
        {
            isWin = true;
        }
    }

    void IncreaseHealth()
    {
        playerHealth += 0.05f;
        if (playerHealth > 1) playerHealth = 1;
        powerbar.fillAmount = playerHealth;
    }

    void DecreaseHealth(int damage)
    {
        playerHealth -= damage;
        powerbar.fillAmount = playerHealth;
        if(playerHealth <= 0)
        {
            //lose
        }
    }

    IEnumerator winplayer()
    {
        //FindObjectOfType<UiManager>().wineffet.SetActive(true);
        GetComponent<Animator>().Play("win");
        GetComponent<Animator>().speed = 1;
        //SoundManager.instance.stop("punch");
        //SoundManager.instance.Play("win");
        yield return new WaitForSeconds(7f);
        //FindObjectOfType<UiManager>().winpanel.SetActive(true);
    }

    IEnumerator dieplayer()
    {
        GetComponent<Animator>().Play("die1");
        GetComponent<Animator>().speed = 1;
        //SoundManager.instance.stop("punch");
        //SoundManager.instance.Play("lose");
        yield return new WaitForSeconds(7f);
        //FindObjectOfType<UiManager>().losepanel.SetActive(true);
    }


}
