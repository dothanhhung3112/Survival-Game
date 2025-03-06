using DG.Tweening;
using Hung;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPrisionEscape : MonoBehaviour
{
    public bool isDie;
    Animator animator;
    NavMeshAgent agent;
    Transform target;
    SkinnedMeshRenderer meshRenderer;
    Material[] originalMats;
    bool isAttacked, isMovingToTarget;
    VisionCone radar;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        radar = GetComponentInChildren<VisionCone>();
        originalMats = meshRenderer.materials;
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

    private void Update()
    {
        if (PrisionEscapeController.instance.isWin || PrisionEscapeController.instance.isLose) return;
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

    public void MoveToTarget(GameObject target)
    {
        agent.SetDestination(target.transform.position);
    }

    public void Attack()
    {
        agent.isStopped = true;
        animator.SetTrigger("Hit");
        SoundManager.Instance.PlaySoundPunch();
        DOVirtual.DelayedCall(0.4f, delegate
        {
            animator.Play("die1");
            SoundManager.Instance.PlaySoundMaleHited();
            SetColorGray(true);
            ObjectPooler.instance.SetObject("bloodEffect", transform.position + new Vector3(0, 0.5f, 0));
            radar.gameObject.SetActive(false);
        });
        DOVirtual.DelayedCall(2f, delegate
        {
            gameObject.SetActive(false);
        });
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        agent.speed = 4f;
        isMovingToTarget = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isDie || isMovingToTarget) return;
        if (other.CompareTag("Bot") || other.CompareTag("Player"))
        {
            isDie = true;
            radar.gameObject.SetActive(false);
            SoundManager.Instance.PlaySoundMaleHited();
            ObjectPooler.instance.SetObject("bloodEffect", transform.position + new Vector3(0, 0.5f, 0));
            animator.Play("die1");
            SetColorGray(true);
        }
    }
}
