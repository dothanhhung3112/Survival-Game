using UnityEngine;
using Hung.Gameplay.GreenRedLight;

namespace Hung.UI
{
    public class UIGreenRedLightController : MonoBehaviour
    {
        public static UIGreenRedLightController Instance;
        public UIWin UIWin { get { return GetComponentInChildren<UIWin>(); } }
        public UILose UILose { get { return GetComponentInChildren<UILose>(); } }
        public UIGamePlay UIGamePlay { get { return GetComponentInChildren<UIGamePlay>(); } }
        public UIMenu UIMenu { get { return GetComponentInChildren<UIMenu>(); } }
        public float time = 65;
        [HideInInspector] public bool canCountTime;
        float timeSound;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            NativeAdsController.Instance.miniGame = NativeAdsController.MiniGame.GreenRedLight;
            UIMenu.DisplayPanelMenu(true);
            UIMenu.SetActionStartGame(delegate
            {
                UIGamePlay.DisplayPanelGameplay(true);
                canCountTime = true;
                FindObjectOfType<PlayerController>().GmRun = true;
            });

            UIRevive.Instance.SetReviveAction(delegate
            {
                time += 10;
                canCountTime = true;
                UIGamePlay.DisplayPanelGameplay(true);
                GreenRedLightController.Instance.player.Revive();
            });
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
    }
}
