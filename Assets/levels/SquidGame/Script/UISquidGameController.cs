using Hung.UI;
using UnityEngine;

public class UISquidGameController : MonoBehaviour
{
    public static UISquidGameController Instance;
    public UIWin UIWin { get { return GetComponentInChildren<UIWin>(); } }
    public UILose UILose { get { return GetComponentInChildren<UILose>(); } }
    public UIGamePlay UIGamePlay { get { return GetComponentInChildren<UIGamePlay>(); } }
    public UIMenu UIMenu { get { return GetComponentInChildren<UIMenu>(); } }
    public UISetting UISetting { get { return GetComponentInChildren<UISetting>(); } }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        NativeAdsController.Instance.miniGame = NativeAdsController.MiniGame.SquidGame;
        UIMenu.DisplayPanelMenu(true);
        UIMenu.SetActionStartGame(delegate
        {
            UIGamePlay.DisplayPanelGameplay(true);
            SquidGameController.Instance.StartGame();
        });

        UIRevive.Instance.SetReviveAction(delegate
        {
            UIGamePlay.DisplayPanelGameplay(true);
            SquidGameController.Instance.Revive();
        });

        UIRevive.Instance.SetOnCloseAction(delegate
        {
            UILose.DisplayPanelLose(true);
        });
    }
}
