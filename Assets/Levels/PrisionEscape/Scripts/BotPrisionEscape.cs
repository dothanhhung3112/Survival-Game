using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BotPrisionEscape : MonoBehaviour
{
    PlayerPrisionEscape player;
    Transform target;
    NavMeshAgent agent;
    Animator animator;
    public float radius;
    public bool canFollow = false;
    public bool canGetPos = true;
    public bool canTele;
    bool isDie = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetPlayer(PlayerPrisionEscape playerPrisionEscape)
    {
        player = playerPrisionEscape;
        canFollow = true;
    }

    private void Update()
    {
        if (canFollow && !isDie)
        {
            float speed = agent.speed;
            agent.Move(player.scaledMovement);
            transform.rotation = player.transform.rotation;
            animator.SetFloat("Speed", player.GetSpeed());
            CheckDistance();
        }

        if (target != null && !isDie)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            animator.SetFloat("Speed", Mathf.Clamp01(agent.velocity.magnitude));
            if (distance < 0.3f)
            {
                Attack();
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
            bot.SetPlayer(player);
            PrisionEscapeController.instance.bots.Add(bot);
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
