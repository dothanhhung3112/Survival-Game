using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWin : MonoBehaviour
{
    [SerializeField] GameObject winPanel;

    public void DisplayPanelWin(bool enable)
    {
        if (enable)
        {
            winPanel.SetActive(true);
        }
        else
        {
            winPanel.SetActive(false);
        }
    }

    public void OnCLickButtonWatchAds()
    {
        GameManager.Instance.LoadNextLevel();
    }

    public void OnClickButtonClaim()
    {
        GameManager.Instance.LoadNextLevel();
    }
}
