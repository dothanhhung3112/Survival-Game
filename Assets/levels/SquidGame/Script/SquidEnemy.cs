using DG.Tweening;
using Hung;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SquidEnemy : MonoBehaviour
{
    [SerializeField] SquidGamePlayer player;
    [SerializeField] Image powerBar;
    [SerializeField] Transform endPos;
    [SerializeField] float enemyHealh;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void kicking()
    {
        SoundManager.Instance.PlaySoundPunch();
        player.DecreaseHealth(0.1f);
    }

    public void MovingToEndPos()
    {
        animator.Play("Running");
        transform.DOMove(new Vector3(endPos.position.x, transform.position.y, endPos.position.z), 2)
        .OnComplete(delegate
        {
            SquidGameController.Instance.canFight = true;
            animator.Play("RightHook");
        });
    }

    public void DecreaseHealth(float damage)
    {
        enemyHealh -= damage;
        powerBar.fillAmount = enemyHealh;
        ObjectPooler.instance.SetObject("bloodEffect", transform.position + new Vector3(0, 0.8f, 0));
        if (enemyHealh <= 0)
        {
            SquidGameController.Instance.Win();
        }
    }

    public void Die()
    {
        animator.Play("Die");
    }

    public void ResetAnim(bool isFighting)
    {
        animator.Play(isFighting?"RightHook":"IdleRunning");
    }

    public void Cheer()
    {
        animator.Play("Cheer");
    }
}
