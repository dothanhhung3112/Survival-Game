using DG.Tweening.Core.Easing;
using UnityEngine;

public class UIGreenRedLightController : MonoBehaviour
{
    public static UIGreenRedLightController Instance;
    public UIWin UIWin { get { return GetComponentInChildren<UIWin>(); } }
    public UILose UILose { get { return GetComponentInChildren<UILose>(); } }
    public UIGamePlay UIGamePlay { get { return GetComponentInChildren<UIGamePlay>(); } }
    public UIMenu UIMenu { get { return GetComponentInChildren<UIMenu>(); } }
    public float time = 65;
    public bool canCountTime;
    public float timeSound;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (canCountTime)
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
                UIGamePlay.SetTimeText(a);
            }
        }
    }

    public void StartButton()
    {
        UIMenu.DisplayPanelMenu(false);
        UIGamePlay.DisplayPanelGameplay(true);
        canCountTime = true;
        FindObjectOfType<PlayerController>().GmRun = true;
    }
}
