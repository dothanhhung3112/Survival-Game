using Hung.UI;
using UnityEngine;

public class UIPrisionEscapeController : MonoBehaviour
{
    public static UIPrisionEscapeController Instance;
    public UIWin UIWin { get { return GetComponentInChildren<UIWin>(); } }
    public UILose UILose { get { return GetComponentInChildren<UILose>(); } }
    public UIGamePlay UIGamePlay { get { return GetComponentInChildren<UIGamePlay>(); } }
    public UIMenu UIMenu { get { return GetComponentInChildren<UIMenu>(); } }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        NativeAdsController.Instance.miniGame = NativeAdsController.MiniGame.PrisionEscape;
        UIMenu.DisplayPanelMenu(true);
        UIMenu.SetActionStartGame(delegate
        {
            UIGamePlay.DisplayPanelGameplay(true);
            PrisionEscapeController.instance.StartGame();
        });

        UIRevive.Instance.SetReviveAction(delegate
        {
            UIGamePlay.DisplayPanelGameplay(true);
            PrisionEscapeController.instance.Revive();
        });

        UIRevive.Instance.SetOnCloseAction(delegate
        {
            UILose.DisplayPanelLose(true);
        });
    }
}
