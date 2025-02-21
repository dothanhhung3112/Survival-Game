using Hung;
using System.Collections;
using UnityEngine;

public class SquidGameController : MonoBehaviour
{
    public static SquidGameController Instance;
    [SerializeField] SquidEnemy enemy;
    [SerializeField] SquidGamePlayer player;
    [SerializeField] GameObject camEnd;
    [SerializeField] ParticleSystem effectWin;
    public bool gamerun, isWin,isLose,canFight;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void Win()
    {
        isWin = true;
        StartCoroutine(winplayer());
    }

    public void Lose()
    {
        isLose = true;
        StartCoroutine(dieplayer());
    }

    public void ChangingToFight()
    {
        player.MoveToBoss(delegate
        {
            enemy.MovingToEndPos();
            camEnd.SetActive(true);
        });
    }

    IEnumerator winplayer()
    {
        enemy.Die();
        effectWin.Play();
        SoundManager.Instance.PlaySoundWin();
        yield return new WaitForSeconds(5f);
        UISquidGameController.Instance.UIWin.DisplayPanelWin(true);
    }

    IEnumerator dieplayer()
    {
        SoundManager.Instance.PlaySoundLose();
        yield return new WaitForSeconds(5f);
        UISquidGameController.Instance.UILose.DisplayPanelLose(true);
    }

    public void StartGame()
    {
        gamerun = true;
        player.StartMoving();
    }
}
