using Cinemachine;
using DG.Tweening;
using Hung;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SquidGamePlayer : MonoBehaviour
{
    [SerializeField] Transform endPos;
    [SerializeField] SquidEnemy enemy;
    [SerializeField] GameObject shieldObject;
    public float speed, speedForward;
    public Image powerbar;
    bool stopMove, isDragging;
    Vector3 presspos, actualpos;
    float playerHealth = 0;
    Rigidbody rb;
    Animator animator;
    int speedToHash;
    bool haveShield;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        speedToHash = Animator.StringToHash("Speed");
    }

    private void Update()
    {
        if (SquidGameController.Instance.isWin || SquidGameController.Instance.isLose) return;
        if (Input.GetMouseButtonDown(0) && SquidGameController.Instance.canFight)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("CrossPunch0") && stateInfo.normalizedTime < 1.0f)
                return;
            animator.Play("CrossPunch0");
            SoundManager.Instance.PlaySoundPunch();
        }

        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            presspos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {  
            isDragging = false;
        }
    }

    public void StartMoving()
    {
        animator.SetFloat(speedToHash, 1);
        animator.speed = 1f;
    }

    float elapsedTime;
    void FixedUpdate()
    {
        if (SquidGameController.Instance.isWin || SquidGameController.Instance.isLose) return;
        if (SquidGameController.Instance.gamerun && !SquidGameController.Instance.isWin && !stopMove)
        {
            rb.MovePosition(rb.position + new Vector3(0, 0, speedForward * Time.deltaTime));

                elapsedTime += Time.deltaTime;
                if (elapsedTime > 0.5f)
                {
                    SoundManager.Instance.PlaySoundWalk();
                    elapsedTime = 0;
                }
        }

        if (Input.GetMouseButton(0) && !stopMove && isDragging)
        {
            actualpos = Input.mousePosition;
            float ss = actualpos.x - presspos.x;
            float xdiff = (actualpos.x - presspos.x) * Time.deltaTime * speed;
            Vector3 tmp = rb.position;
            tmp.x += xdiff;
            tmp.x = Mathf.Clamp(tmp.x, -6f, 6f);
            rb.MovePosition(Vector3.Lerp(rb.position, tmp, speed * Time.deltaTime));
            presspos = actualpos;
        }
    }

    public void DecreaseEnemyHealth()
    {
        enemy.DecreaseHealth(0.3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (SquidGameController.Instance.isWin || SquidGameController.Instance.isLose) return;
        if (collision.gameObject.tag == "food")
        {
            SoundManager.Instance.PlaySoundEatMeat();
            Destroy(collision.gameObject);
            IncreaseHealth();
        }
        if (collision.gameObject.tag == "Obstacle" && !haveShield)
        {
            Vector3 hitPoint = collision.contacts[0].point;
            Vector3 centerPoint = collision.transform.position;
            if(hitPoint.x < centerPoint.x)
            {
                Vector3 direction = (transform.position + new Vector3(-0.5f,0,0)) - collision.transform.position;
                direction.y = 0;
                rb.AddForce(direction.normalized * 100, ForceMode.Impulse);
            }
            else
            {
                Vector3 direction = (transform.position + new Vector3(0.5f, 0, 0)) - collision.transform.position;
                direction.y = 0;
                rb.AddForce(direction.normalized * 100, ForceMode.Impulse);
            }
            SoundManager.Instance.PlaySoundMaleHited();
            DecreaseHealth(0.05f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "win")
        {
            SquidGameController.Instance.ChangingToFight();
        }
    }

    public void MoveToBoss(Action onDoneMove = null)
    {
        stopMove = true;
        animator.speed = 0.7f;
        transform.LookAt(endPos);
        transform.DOMove(new Vector3(endPos.position.x, transform.position.y, endPos.position.z), 1.5f).SetEase(Ease.Linear)
        .OnComplete(delegate
        {
            animator.SetFloat(speedToHash, 0);
            animator.speed = 1f;
            animator.Play("Idle");
            transform.DOLookAt(new Vector3(enemy.transform.position.x, transform.position.y, enemy.transform.position.z),0.5f);
            onDoneMove?.Invoke();
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
        ObjectPooler.instance.SetObject("bloodEffect", transform.position + new Vector3(0, 0.8f, 0));
        if (playerHealth <= 0)
        {
            SquidGameController.Instance.Lose();
        }
    }

    public void RevivePLayer()
    {
        if (!stopMove)
        {
            rb.isKinematic = false;
            animator.Play("IdleRunning");
            enemy.ResetAnim(false);
            StartCoroutine(ActiveShield3Sec());
        }
        else
        {
            rb.isKinematic = false;
            animator.Play("Idle");
            playerHealth += 0.5f;
            powerbar.fillAmount = playerHealth;
            enemy.ResetAnim(true);
        }
    }

    IEnumerator ActiveShield3Sec()
    {
        haveShield = true;
        shieldObject.SetActive(true);
        float time = 0;
        while(time < 3)
        {
            time += Time.deltaTime;
            yield return null;
        }
        haveShield = false;
        shieldObject.SetActive(false);
    }

    public void Die()
    {
        rb.isKinematic = true;
        animator.Play("Die");
        transform.position += new Vector3(0, 0.15f, 0);
    }

    public void Cheer()
    {
        animator.Play("Cheer");
    }
}
