using UnityEngine;
using UnityEngine.AI;

public class BotPrisionEscape : MonoBehaviour
{
    PlayerPrisionEscape target;
    NavMeshAgent Agent;
    Animator animator;
    public float radius;
    float lengthCircle = 4;
    public bool canFollow = false;
    Vector3 lastPos;
    public bool canGetPos = true;
    public bool canTele;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Agent = GetComponent<NavMeshAgent>();
    }

    public void SetActive(PlayerPrisionEscape player)
    {
        target = player;
        canFollow = true;
    }

    private void Update()
    {
        if (canFollow)
        {
            float speed = Agent.speed;
            Agent.Move(target.scaledMovement);
            transform.rotation = target.transform.rotation;
            animator.SetFloat("Speed", target.GetSpeed());
            CheckDistance();
        }
    }

    void CheckDistance()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > 3)
        {
            Vector3 direction = (transform.position - target.transform.position).normalized;
            transform.position = target.transform.position + direction * 3;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bot"))
        {
            other.tag = "Player";
            BotPrisionEscape bot = other.GetComponent<BotPrisionEscape>();
            bot.SetActive(target);
        }
    }
}
