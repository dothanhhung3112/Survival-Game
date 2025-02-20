using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidGameController : MonoBehaviour
{
    public static SquidGameController Instance;
    public bool gamerun, isWin;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void Win()
    {
        StartCoroutine(winplayer());
    }

    public void Lose()
    {
        StartCoroutine(dieplayer());
    }

    IEnumerator winplayer()
    {
        //FindObjectOfType<UiManager>().wineffet.SetActive(true);
        GetComponent<Animator>().Play("win");
        GetComponent<Animator>().speed = 1;
        //SoundManager.instance.stop("punch");
        //SoundManager.instance.Play("win");
        yield return new WaitForSeconds(7f);
        //FindObjectOfType<UiManager>().winpanel.SetActive(true);
    }

    IEnumerator dieplayer()
    {
        GetComponent<Animator>().Play("die1");
        GetComponent<Animator>().speed = 1;
        //SoundManager.instance.stop("punch");
        //SoundManager.instance.Play("lose");
        yield return new WaitForSeconds(7f);
        //FindObjectOfType<UiManager>().losepanel.SetActive(true);
    }
}
