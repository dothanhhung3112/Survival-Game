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
        UIMenu.DisplayPanelMenu(true);
    }

    public void StartButton()
    {
        PrisionEscapeController.instance.StartGame();
        UIMenu.DisplayPanelMenu(false);
        UIGamePlay.DisplayPanelGameplay(true);
    }
}
