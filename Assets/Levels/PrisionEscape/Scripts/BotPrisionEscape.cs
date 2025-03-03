using DG.Tweening;
using Hung;
using UnityEngine;
using UnityEngine.AI;

public class BotPrisionEscape : MonoBehaviour
{
    public bool isDie =false;
    [SerializeField] PlayerPrisionEscape player;
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
    }

    private void Start()
    {
        originalMats = meshRenderer.materials;
        SetColorGray(true);
    }

    public void SetColorGray(bool isGray)
    {
        Material[] mat = meshRenderer.materials;
        for(int i = 0;i < mat.Length; i++)
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
        if (canFollow && !isDie && player != null)
        {
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
            animator.SetFloat("Speed", Mathf.Clamp01(agent.velocity.magnitude));
            if (distance < 0.8f)
            {
                Attack();
                isDie = true;
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
            animator.Play("die1");
            SetColorGray(true);
            ObjectPooler.instance.SetObject("bloodEffect", transform.position + new Vector3(0, 0.5f, 0));
        });
    }

    void CheckDistance()
    {
        float maxDistance = 3.5f;
        float safeDistance = 3f;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bot"))
        {
            BotPrisionEscape bot = other.GetComponent<BotPrisionEscape>();
            if (bot.isDie || !canFollow) return;
            bot.SetPlayer(player);
            bot.SetColorGray(false);
            PrisionEscapeController.instance.bots.Add(bot);
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        agent.speed = 1.5f;
    }  
}
