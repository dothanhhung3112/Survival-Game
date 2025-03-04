using DG.Tweening;
using Hung;
using UnityEngine;
using UnityEngine.AI;

public class BotPrisionEscape : MonoBehaviour
{
    public bool isDie = false;
    [SerializeField] PlayerPrisionEscape player;
    [SerializeField] DOTweenPath pathWin;
    Transform target;
    NavMeshAgent agent;
    Animator animator;
    SkinnedMeshRenderer meshRenderer;
    Material[] originalMats;
    bool canFollow;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        pathWin = GetComponent<DOTweenPath>();
    }

    private void Start()
    {
        int randomeDance = Random.Range(1, 5);
        animator.Play("Dance" + randomeDance);

        originalMats = meshRenderer.materials;
        SetColorGray(true);
    }

    public void SetColorGray(bool isGray)
    {
        Material[] mat = meshRenderer.materials;
        for (int i = 0; i < mat.Length; i++)
        {
            mat[i] = isGray ? PrisionEscapeController.instance.grayMat : originalMats[i];
        }
        meshRenderer.materials = mat;
    }

    public void SetPlayer(PlayerPrisionEscape playerPrisionEscape)
    {
        player = playerPrisionEscape;
        canFollow = true;
    }

    private void Update()
    {
        if (PrisionEscapeController.instance.isWin || PrisionEscapeController.instance.isLose) return;

        animator.SetFloat("Speed", Mathf.Clamp01(agent.velocity.magnitude));
        if (canFollow && !isDie && player != null)
        {
            animator.Play("RunFight");
            float speed = agent.speed;
            agent.Move(player.scaledMovement);
            transform.rotation = player.transform.rotation;
            animator.SetFloat("Speed", player.GetSpeed());
            CheckDistance();
        }

        if (target != null && !isDie)
        {
            canFollow = false;
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance < 0.8f)
            {
                isDie = true;
                Attack();
                PrisionEscapeController.instance.bots.Remove(this);
            }
            else
            {
                agent.SetDestination(target.position);
                transform.LookAt(target);
            }
        }
    }

    public void Attack()
    {
        agent.isStopped = true;
        animator.SetTrigger("Hit");
        DOVirtual.DelayedCall(0.4f, delegate
        {
            Die();
        });
    }

    public void Die()
    {
        isDie = true;
        animator.Play("die1");
        ObjectPooler.instance.SetObject("bloodEffect", transform.position + new Vector3(0, 0.5f, 0));
        SetColorGray(true);
        DOVirtual.DelayedCall(2f, delegate
        {
            gameObject.SetActive(false);
        });
    }

    void CheckDistance()
    {
        float maxDistance = 2f;
        float safeDistance = 1.5f;
        float checkRadius = 1.0f;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance > maxDistance)
        {
            Vector3 direction = (transform.position - player.transform.position).normalized;
            Vector3 newPosition = player.transform.position + direction * safeDistance;
            if (NavMesh.SamplePosition(newPosition, out NavMeshHit hit, checkRadius, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
            }
        }
    }

    public void MoveToCar(Vector3[] path)
    {
        animator.Play("run");
        transform.DOPath(path, 2f).SetEase(Ease.Linear).SetLookAt(0.1f);
        DOVirtual.DelayedCall(2f, () => gameObject.SetActive(false));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bot"))
        {
            BotPrisionEscape bot = other.GetComponent<BotPrisionEscape>();
            if (bot.isDie || !canFollow || PrisionEscapeController.instance.bots.Contains(bot)) return;
            bot.SetPlayer(player);
            bot.SetColorGray(false);
            PrisionEscapeController.instance.bots.Add(bot);
        }

        if (other.CompareTag("win"))
        {
            PrisionEscapeController.instance.StartEndCard();
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        agent.speed = 1.5f;
    }
}
