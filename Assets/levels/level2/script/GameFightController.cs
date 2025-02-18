using DG.Tweening;
using Hung;
using System.Collections.Generic;
using UnityEngine;

public class GameFightController : MonoBehaviour
{
    #region singleton
    public static GameFightController Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    #endregion

    [SerializeField] PlayerControllerLvFight player;
    [SerializeField] List<EnemyControllerLvFight> enemys;
    [SerializeField] ParticleSystem effectWin;
    [SerializeField] float time;
    public bool isWin = false;
    public bool isLose = false;
    public bool canCountTime = false;
    float botSum;
    float timeSound;

    private void Start()
    {
        botSum = enemys.Count;
        UIFightController.Instance.UIGamePlay.SetBotLeftText(botSum,botSum);
    }

    private void Update()
    {
        if (canCountTime && !isLose && !isWin)
        {
            time -= Time.deltaTime;
            timeSound -= Time.deltaTime;
            if (timeSound <= 0 && time >= 0)
            {
                timeSound = 1;
                SoundManager.Instance.PlaySoundTimeCount();
            }
            int a = (int)time;
            if (a >= 0)
            {
                UIFightController.Instance.UIGamePlay.SetTimeText(a);
            }
            else
            {
                Lose();
            }
        }
    }

    public void UpdateEnemy(EnemyControllerLvFight enemy)
    {
        enemys.Remove(enemy);
        if (enemys.Count == 0)
        {
            Win();
        }
        UIFightController.Instance.UIGamePlay.SetBotLeftText(enemys.Count, botSum);
    }

    public bool IsEnemysContain(GameObject go)
    {
        if(isWin || isLose) return false;
        if (enemys.Count == 0) return false;

        for(int i = 0; i <= enemys.Count; i++)
        {
            if (enemys[i].gameObject == go) return true;
        }
        return false;
    }

    void Win()
    {
        effectWin.gameObject.SetActive(true);
        effectWin.Play();
        player.Win();
        isWin = true;
        DOVirtual.DelayedCall(6f, delegate
        {
            UIFightController.Instance.UIGamePlay.DisplayPanelGameplay(false);
            UIFightController.Instance.UIWin.DisplayPanelWin(true);
        });
    }

    void Lose()
    {
        isLose = true;
        UIFightController.Instance.UIGamePlay.DisplayPanelGameplay(false);
        UIFightController.Instance.UILose.DisplayPanelLose(true);
    }

    public void StartGame()
    {
        canCountTime = true;
        foreach(var item in enemys)
        {
            item.StartMoving();
        }
    }
}
