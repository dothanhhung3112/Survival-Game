using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Hung.Gameplay.GameFight
{
    public class EnemyControllerLvFight : MonoBehaviour
    {
        NavMeshAgent navMeshAgent;
        Animator animator;
        Coroutine movingRandom;
        Vector3 nextPos;
        int speedToHash;
        bool isDie = false;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            nextPos = transform.position;
            speedToHash = Animator.StringToHash("Speed");
        }

        private void Update()
        {
            if (GameFightController.Instance.isLose || GameFightController.Instance.isWin) return;
            animator.SetFloat(speedToHash, Mathf.Clamp01(navMeshAgent.velocity.magnitude));
        }

        public void StartMoving()
        {
            movingRandom = StartCoroutine(MovingRandom());
        }

        IEnumerator MovingRandom()
        {
            while (!isDie || !GameFightController.Instance.isLose || !GameFightController.Instance.isWin)
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
            if (isDie) return;
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
}
