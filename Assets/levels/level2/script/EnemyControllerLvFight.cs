using DG.Tweening;
using Hung;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControllerLvFight : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Vector3 nextPos;
    private int speedToHash;
    private bool isDie = false;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        nextPos = transform.position;
        speedToHash = Animator.StringToHash("Speed");
        StartCoroutine(StartMovingRandom());
    }

    private void Update()
    {
        animator.SetFloat(speedToHash,Mathf.Clamp01(navMeshAgent.velocity.magnitude));
    }

    IEnumerator StartMovingRandom()
    {
        while (!isDie)
        {
            nextPos = GetRandomPoint(transform.position, 20);
            navMeshAgent.SetDestination(nextPos);
            yield return new WaitForSeconds(6f);  
        }
    }

    private Vector3 GetRandomPoint(Vector3 startPos, float radius)
    {
        Vector3 dir = Random.insideUnitSphere * radius;
        dir += startPos;
        NavMeshHit hit;
        Vector3 finalPos = Vector3.zero;
        if (NavMesh.SamplePosition(dir, out hit, radius, 1))
        {
            finalPos = hit.position;
        }
        return finalPos;
    }

    public void Die()
    {
        isDie = true;
        navMeshAgent.isStopped = true;
        animator.Play("die1");
        GameObject gm = ObjectPooler.instance.SetObject("bloodEffect", transform.position);
        DOVirtual.DelayedCall(4f, delegate
        {
            gameObject.SetActive(false);
        });
    }
}
