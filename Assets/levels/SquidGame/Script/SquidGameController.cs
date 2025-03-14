using DG.Tweening;
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

    private void Start()
    {
        SoundManager.Instance.PlayBGMusic5();
    }

    public void Win()
    {
        if (isWin || isLose) return;
        isWin = true;
        StartCoroutine(winplayer());
    }

    public void Lose()
    {
        if (isWin || isLose) return;
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
        UISquidGameController.Instance.UIGamePlay.DisplayPanelGameplay(false);
        enemy.Die();
        effectWin.Play();
        SoundManager.Instance.StopMusic();
        SoundManager.Instance.PlaySoundWin();
        player.transform.DOLookAt(new Vector3(camEnd.transform.position.x, 0, camEnd.transform.position.z), 0.8f).OnComplete(delegate
        {
            player.Cheer();
        });
        yield return new WaitForSeconds(5f);
        UISquidGameController.Instance.UIWin.DisplayPanelWin(true);
    }

    IEnumerator dieplayer()
    {
        UISquidGameController.Instance.UIGamePlay.DisplayPanelGameplay(false);
        player.Die();
        yield return new WaitForSeconds(0.5f);
        enemy.Cheer();
        SoundManager.Instance.StopMusic();
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
