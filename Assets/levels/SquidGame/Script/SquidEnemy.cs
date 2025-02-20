using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SquidEnemy : MonoBehaviour
{
    public Transform mnplayer;
    public bool kick, canFight, isDie;
    [SerializeField] float enemyHealh;
    void Start()
    {
        mnplayer = FindObjectOfType<SquidGamePlayer>().GetComponent<Transform>();
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
            int bb = Random.Range(1, 4);
            GetComponent<Animator>().Play("kick" + bb.ToString());
            GetComponent<Animator>().speed = 2;
            mnplayer.GetChild(2).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().fillAmount -= Random.Range(0.1f, 0.025f);
            yield return new WaitForSeconds(0.6f);
        }

    }


}
