using UnityEngine;
using UnityEngine.AI;

public class EnemyPrisionEscape : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;
    Transform target;
    bool isDie = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(target != null && !isDie)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            animator.SetFloat("Speed",Mathf.Clamp01(agent.velocity.magnitude));
            if(distance < 0.3f)
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

    public void MoveToTarget(GameObject target)
    {
        agent.SetDestination(target.transform.position);
    }

    public void Attack()
    {
        agent.isStopped = true;
        animator.SetTrigger("Hit");
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
