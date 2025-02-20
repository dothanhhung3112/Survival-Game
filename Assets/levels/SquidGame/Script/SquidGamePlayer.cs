using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SquidGamePlayer : MonoBehaviour
{
    [SerializeField] Transform endPos;
    public float speed, speedForward;
    public Image powerbar;
    public Transform boss;
    bool die, kick, win;
    Vector3 presspos, actualpos;
    float playerHealth = 0;
    bool stopMove = false;
    Rigidbody rb;
    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && !SquidGameController.Instance.gamerun)
        {
            SquidGameController.Instance.gamerun = true;
            animator.Play("Running");
            animator.speed = 0.7f;
            presspos = Input.mousePosition;
            //FindObjectOfType<UiManager>().startpanel.SetActive(false);
        }

        if (SquidGameController.Instance.gamerun && !SquidGameController.Instance.isWin && !stopMove)
        {
            rb.MovePosition(rb.position + new Vector3(0, 0, speedForward * Time.deltaTime));
        }

        if (Input.GetMouseButton(0) && !stopMove)
        {
            actualpos = Input.mousePosition;
            float ss = actualpos.x - presspos.x;
            ss = Mathf.Clamp(ss, -1, 1);
            float xdiff = (actualpos.x - presspos.x) * Time.deltaTime * speed;
            Vector3 tmp = transform.position;
            tmp.x += xdiff;
            tmp.x = Mathf.Clamp(tmp.x, -4.5f, 3f);
            rb.MovePosition(Vector3.Lerp(rb.position, tmp, speed * Time.deltaTime));
            presspos = actualpos;
        }


        //if (SquidGameController.Instance.isWin)
        //{
        //    if (Vector3.Distance(transform.position, boss.position) >= 1.2f)
        //    {
        //        GetComponent<Animator>().Play("run");
        //        transform.LookAt(boss);
        //        transform.position = Vector3.MoveTowards(transform.position, boss.position, 7 * Time.deltaTime);
        //    }
        //    else
        //    {
        //        if (!SquidGameController.Instance.stopfolow)
        //        {
        //            fightpanel.SetActive(true);
        //            GetComponent<Animator>().Play("idle1");
        //            SoundManager.instance.Play("punch");
        //            FindObjectOfType<SquidEnemy>().canFight = true;
        //        }
        //        SquidGameController.Instance.stopfolow = true;
        //        transform.LookAt(boss);
        //        Vector3 v = boss.position;
        //        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, campos.position, 7 * Time.deltaTime);
        //        Camera.main.transform.LookAt(look);
        //    }
        //}

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
        if (collision.gameObject.tag == "Obstacle")
        {
            Vector3 direction = transform.position - collision.transform.position;
            direction.y = 0;
            DecreaseHealth(0.05f);
            rb.AddForce(direction.normalized * 100, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "win")
        {
            SquidGameController.Instance.isWin = true;
            MoveToBoss();
        }
    }

    void MoveToBoss()
    {
        stopMove = true;
        transform.DOMove(new Vector3(endPos.position.x, transform.position.y, endPos.position.z), 3f)
        .OnComplete(delegate
        {
            animator.Play("Idle");
        });
    }

    void IncreaseHealth()
    {
        playerHealth += 0.05f;
        if (playerHealth > 1) playerHealth = 1;
        powerbar.fillAmount = playerHealth;
    }

    public void DecreaseHealth(float damage)
    {
        playerHealth -= damage;
        powerbar.fillAmount = playerHealth;
        if (playerHealth <= 0)
        {
            SquidGameController.Instance.Lose();
        }
    }
}
