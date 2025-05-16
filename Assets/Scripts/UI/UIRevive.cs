using ACEPlay.Bridge;
using Hung;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIRevive : MonoBehaviour
{
    public static UIRevive Instance;
    [SerializeField] GameObject popupRevivePanel;
    [SerializeField] TextMeshProUGUI textCountDown;
    [SerializeField] Image imageCountDown;
    Coroutine showRevivePopup;
    Action reviveAction;
    Action onClosePanel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void DisplayRevivePanel(bool enable)
    {
        if (enable)
        {
            AdBreaks.instance.StopCountDownShowAdBreaks();
            popupRevivePanel.SetActive(true);
            showRevivePopup = StartCoroutine(ShowRevivePopup());
        }
        else
        {
            popupRevivePanel.SetActive(false);
        }
    }

    IEnumerator ShowRevivePopup()
    {
        float time = 6;
        while (time > 0)
        {
            textCountDown.text = $"{(int)time}";
            imageCountDown.fillAmount = time / 5;
            time -= Time.unscaledDeltaTime;
            yield return null;
        }
        Manager.Instance.isRevived = true;
        DisplayRevivePanel(false);
        onClosePanel?.Invoke();
    }

    public void OnClickButtonRevive()
    {

        if (BridgeController.instance.IsRewardReady())
        {
            if (showRevivePopup != null)
            {
                StopCoroutine(showRevivePopup);
                showRevivePopup = null;
            }

            UnityEvent e = new UnityEvent();
            e.AddListener(() =>
            {
                popupRevivePanel.SetActive(false);
                Manager.Instance.isRevived = true;
                reviveAction?.Invoke();
            });
            BridgeController.instance.ShowRewarded("rewardrevice", e);
        }
    }

    public void OnClickButtonNothanks()
    {
        StopCoroutine(showRevivePopup);
        popupRevivePanel.gameObject.SetActive(false);
        Manager.Instance.isRevived = true;
        onClosePanel?.Invoke();
    }

    public void SetReviveAction(Action action)
    {
        if (showRevivePopup != null)
        {
            StopCoroutine(showRevivePopup);
            showRevivePopup = null;
        }
        reviveAction = action;
    }

    public void SetOnCloseAction(Action action)
    {
        if (showRevivePopup != null)
        {
            StopCoroutine(showRevivePopup);
            showRevivePopup = null;
        }
        onClosePanel = action;
    }
}
