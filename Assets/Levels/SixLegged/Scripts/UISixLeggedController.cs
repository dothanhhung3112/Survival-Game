using Hung.UI;
using UnityEngine;

public class UISixLeggedController : MonoBehaviour
{
    public static UISixLeggedController Instance;
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
        NativeAdsController.Instance.miniGame = NativeAdsController.MiniGame.SixLegged;
        UIMenu.DisplayPanelMenu(true);
        UIMenu.SetActionStartGame(delegate
        {
            SixLeggedController.Instance.canMove = true;
        });

        UIRevive.Instance.SetReviveAction(delegate
        {
            SixLeggedController.Instance.Revive();
        });

        UIRevive.Instance.SetOnCloseAction(delegate
        {
            UILose.DisplayPanelLose(true);
        });
    }
}
