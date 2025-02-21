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
    public float speed, speedForward;
    public Image powerbar;
    bool die, win, stopMove, isDragging;
    Vector3 presspos, actualpos, tmp;
    float playerHealth = 0;
    Rigidbody rb;
    Animator animator;
    float velocity;
    int speedToHash;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        speedToHash = Animator.StringToHash("Speed");
    }

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && SquidGameController.Instance.canFight)
        {
            kicking();
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
        animator.SetFloat("Speed", 1);
        animator.speed = 1f;
    }

    void FixedUpdate()
    {
        if (SquidGameController.Instance.gamerun && !SquidGameController.Instance.isWin && !stopMove)
        {
            rb.MovePosition(rb.position + new Vector3(0, 0, speedForward * Time.deltaTime));
        }

        if (Input.GetMouseButton(0) && !stopMove && isDragging)
        {
            actualpos = Input.mousePosition;
            float ss = actualpos.x - presspos.x;
            float xdiff = (actualpos.x - presspos.x) * Time.deltaTime * speed;
            Vector3 tmp = rb.position;
            tmp.x += xdiff;
            tmp.x = Mathf.Clamp(tmp.x, -5f, 5f);
            rb.MovePosition(Vector3.Lerp(rb.position, tmp, speed * Time.deltaTime));
            presspos = actualpos;
        }
    }

    public void kicking()
    {
        animator.Play("CrossPunch0");
    }

    public void DecreaseEnemyHealth()
    {
        enemy.DecreaseHealth(0.2f);
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
            SquidGameController.Instance.ChangingToFight();
        }
    }

    public void MoveToBoss(Action onDoneMove = null)
    {
        stopMove = true;
        animator.speed = 0.5f;
        transform.LookAt(endPos);
        transform.DOMove(new Vector3(endPos.position.x, transform.position.y, endPos.position.z), 2f)
        .OnComplete(delegate
        {
            animator.speed = 1f;
            animator.Play("Idle");
            transform.DOLookAt(new Vector3(enemy.transform.position.x,0, enemy.transform.position.z),0.5f);
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
        ObjectPooler.instance.SetObject("bloodEffect", transform.position + new Vector3(0, 0.5f, 0));
        if (playerHealth <= 0)
        {
            SquidGameController.Instance.Lose();
        }
    }

    public void Die()
    {
        animator.Play("Die");
    }

    public void Cheer()
    {
        animator.Play("Cheer");
    }
}
