using CnControls;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerPrisionEscape : MonoBehaviour
{
    [SerializeField] SimpleJoystick joystick;
    [SerializeField] List<Transform> botPos;
    GameObject lookAt;
    Animator animator;
    NavMeshAgent agent;
    SkinnedMeshRenderer meshRenderer;
    Transform target;
    int speedToHash;
    bool canMove = false;
    bool isDie;
    public Vector3 scaledMovement;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        speedToHash = Animator.StringToHash("Speed");
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        lookAt = gameObject;
    }

    private void Update()
    {
        if (canMove)
        {
            scaledMovement = agent.speed * Time.deltaTime * new Vector3(joystick.HorizintalAxis.Value, 0, joystick.VerticalAxis.Value);
            agent.Move(scaledMovement);
            transform.LookAt(lookAt.transform.position + scaledMovement);
            animator.SetFloat(speedToHash, GetSpeed());
        }

        if (target != null && !isDie)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            animator.SetFloat("Speed", Mathf.Clamp01(agent.velocity.magnitude));
            if (distance < 0.8f)
            {
                isDie = true;
                Attack();
            }
            else
            {
                agent.SetDestination(target.position);
                transform.LookAt(target);
            }
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        agent.speed = 4f;
    }

    public float GetSpeed()
    {
        float joystickMagnitude = new Vector2(joystick.HorizintalAxis.Value, joystick.VerticalAxis.Value).magnitude;
        return Mathf.Clamp01(joystickMagnitude == 0 ? agent.velocity.magnitude : joystickMagnitude);
    }

    public Vector3 GetLookAtPos()
    {
        return lookAt.transform.position;
    }

    public void Attack()
    {
        agent.isStopped = true;
        animator.SetTrigger("Hit");
        DOVirtual.DelayedCall(0.4f, delegate
        {
            animator.Play("die1");
            SetColorGray();
        });
    }

    public void SetColorGray()
    {
        Material[] mat = meshRenderer.materials;
        for (int i = 0; i < mat.Length; i++)
        {
            mat[i] = PrisionEscapeController.instance.grayMat;
        }
        meshRenderer.materials = mat;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bot"))
        {
            BotPrisionEscape bot = other.GetComponent<BotPrisionEscape>();
            if (bot.isDie) return;
            bot.SetPlayer(this);
            bot.SetColorGray(false);
            PrisionEscapeController.instance.bots.Add(bot);
        }
    }
}
