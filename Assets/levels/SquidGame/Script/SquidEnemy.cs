using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SquidEnemy : MonoBehaviour
{
    public SquidGamePlayer player;
    public bool kick, canFight, isDie;
    [SerializeField] float enemyHealh;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(canFight)
        {
            kicking();
        }
        else if(isDie)
        {
            kick=true;
            GetComponent<Animator>().Play("die1");
        }
    }

    public void kicking()
    {
        StartCoroutine(StartFighting());
    }

    IEnumerator StartFighting()
    {
        while (!isDie)
        {
            animator.Play("Slap");
            player.DecreaseHealth(0.15f);
            yield return new WaitForSeconds(3);
        }

    }


}
