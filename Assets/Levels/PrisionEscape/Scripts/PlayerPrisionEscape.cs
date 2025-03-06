using CnControls;
using DG.Tweening;
using Hung;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerPrisionEscape : MonoBehaviour
{
    [SerializeField] SimpleJoystick joystick;
    [SerializeField] List<Transform> botPos;
    DOTweenPath pathWin;
    GameObject lookAt;
    Animator animator;
    NavMeshAgent agent;
    SkinnedMeshRenderer meshRenderer;
    Transform target;
    int speedToHash;
    bool isDie;
    public Vector3 scaledMovement;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        speedToHash = Animator.StringToHash("Speed");
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        pathWin = GetComponent<DOTweenPath>();
        lookAt = gameObject;
    }

    float elapsedTime;
    private void Update()
    {
        if (PrisionEscapeController.instance.isWin || !PrisionEscapeController.instance.gameStarted || PrisionEscapeController.instance.isLose) return;

        animator.SetFloat("Speed", Mathf.Clamp01(agent.velocity.magnitude));
        scaledMovement = agent.speed * Time.deltaTime * new Vector3(joystick.HorizintalAxis.Value, 0, joystick.VerticalAxis.Value);
        agent.Move(scaledMovement);
        transform.LookAt(lookAt.transform.position + scaledMovement);
        animator.SetFloat(speedToHash, GetSpeed());

        if (scaledMovement.magnitude > 0)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > 0.4f)
            {
                SoundManager.Instance.PlaySoundWalk();
                elapsedTime = 0;
            }
        }

        if (target != null && !isDie)
        {
            float distance = Vector3.Distance(transform.position, target.position);
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
        SoundManager.Instance.PlaySoundPunch();
        DOVirtual.DelayedCall(0.4f, delegate
        {
            PrisionEscapeController.instance.Lose();
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

    public void MoveToCar(Vector3[] path)
    {
        animator.Play("run");
        transform.DOPath(path, 2f).SetEase(Ease.Linear).SetLookAt(0.1f);
        DOVirtual.DelayedCall(2, () => gameObject.SetActive(false));
    }

    public void Die()
    {
        animator.Play("die1");
        SoundManager.Instance.PlaySoundMaleHited();
        ObjectPooler.instance.SetObject("bloodEffect", transform.position + new Vector3(0, 0.5f, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bot"))
        {
            BotPrisionEscape bot = other.GetComponent<BotPrisionEscape>();
            if (bot.isDie || PrisionEscapeController.instance.bots.Contains(bot)) return;
            bot.SetPlayer(this);
            bot.SetColorGray(false);
            PrisionEscapeController.instance.bots.Add(bot);
        }

        if (other.CompareTag("win"))
        {
            PrisionEscapeController.instance.StartEndCard();
        }
    }
}
