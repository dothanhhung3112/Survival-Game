using ACEPlay.Bridge;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AdBreaks : MonoBehaviour
{
    public static AdBreaks instance;
    public float timeElapsedAdBreak = 0;
    [SerializeField] bool canShowBreakAds = true;
    [SerializeField] RectTransform dialog;
    [SerializeField] Image fillBar;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartCountDownShowAdBreaks();
    }

    public void StartCountDownShowAdBreaks()
    {
        if (!canShowBreakAds) return;
        if (!BridgeController.instance.CanShowInterIngame) return;
        countDownShowAdBreaks = StartCoroutine(CountDownShowAdBreaks());
    }

    public void StopCountDownShowAdBreaks()
    {
        if(countDownShowAdBreaks != null)
        {
            StopCoroutine(countDownShowAdBreaks);
            countDownShowAdBreaks = null;
        }
    }

    void DisplayAdBreakDialog(bool enable)
    {
        if (enable)
        {
            dialog.gameObject.SetActive(true);
            dialog.anchoredPosition = new Vector2(250, 450);
            fillBar.fillAmount = 1;
            dialog.DOAnchorPosX(0, 0.8f).SetEase(Ease.OutQuart).OnComplete(delegate
            {
                countDownShowInter = StartCoroutine(CountDownShowInter());
            });
        }
        else
        {
            if (countDownShowInter != null)
            {
                StopCoroutine(countDownShowInter);
                countDownShowInter = null;
            }
            dialog.gameObject.SetActive(false);
        }
    }

    Coroutine countDownShowInter;
    IEnumerator CountDownShowInter()
    {
        float timeCountDown = 3;
        while (timeCountDown >= 0)
        {
            timeCountDown -= Time.unscaledDeltaTime;
            fillBar.fillAmount = timeCountDown / 3;
            yield return null;
        }
        DisplayAdBreakDialog(false);
        Time.timeScale = 0;

        UnityEvent e = new UnityEvent();
        e.AddListener(delegate
        {
            Time.timeScale = 1;
            timeElapsedAdBreak = 0;
            StartCountDownShowAdBreaks();
        });
        BridgeController.instance.ShowInterstitial("adbreaks", e, null, true);
    }

    Coroutine countDownShowAdBreaks;
    IEnumerator CountDownShowAdBreaks()
    {
        while(timeElapsedAdBreak < BridgeController.instance.TimeShowInterIngame - 3.8f || !BridgeController.instance.CheckStatusShowInter())
        {
            timeElapsedAdBreak += Time.deltaTime;
            yield return null;
        }
        DisplayAdBreakDialog(true);
    }
}
