using ACEPlay.Bridge;
using DG.Tweening;
using Hung;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UISetting : MonoBehaviour
{
    public static UISetting Instance;

    [Header("Panel Setting")]
    [SerializeField] private GameObject pnlSetting;
    [SerializeField] private Transform popUpSetting;

    [Header("Music")]
    [SerializeField] private RectTransform toggleMusic;
    [SerializeField] private Image bgSetMusic;
    [SerializeField] private GameObject textMusicOn;
    [SerializeField] private GameObject textMusicOff;

    [Header("Sound")]
    [SerializeField] private RectTransform toggleSound;
    [SerializeField] private Image bgSetSound;
    [SerializeField] private GameObject textSoundOn;
    [SerializeField] private GameObject textSoundOff;

    [Header("Vibrate")]
    [SerializeField] private RectTransform toggleVibrate;
    [SerializeField] private Image bgSetVibrate;
    [SerializeField] private GameObject textVibrateOn;
    [SerializeField] private GameObject textVibrateOff;

    [Header("Sprite")]
    [SerializeField] private Sprite sprBgOn;
    [SerializeField] private Sprite sprBgOff;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void DisplayPanelSetting(bool enable)
    {
        if (enable)
        {
            if (Manager.Instance.IsMuteMusic)
            {
                toggleMusic.anchoredPosition = new Vector2(-60, 0f);
                bgSetMusic.sprite = sprBgOff;
                textMusicOn.SetActive(false);
                textMusicOff.SetActive(true);
            }
            else
            {
                toggleMusic.anchoredPosition = new Vector2(60, 0f);
                bgSetMusic.sprite = sprBgOn;
                textMusicOn.SetActive(true);
                textMusicOff.SetActive(false);
            }

            if (Manager.Instance.IsMuteSound)
            {
                toggleSound.anchoredPosition = new Vector2(-60, 0f);
                bgSetSound.sprite = sprBgOff;
                textSoundOn.SetActive(false);
                textSoundOff.SetActive(true);
            }
            else
            {
                toggleSound.anchoredPosition = new Vector2(60, 0f);
                bgSetSound.sprite = sprBgOn;
                textSoundOn.SetActive(true);
                textSoundOff.SetActive(false);
            }

            if (Manager.Instance.IsOffVibration)
            {
                toggleVibrate.anchoredPosition = new Vector2(-60, 0f);
                bgSetVibrate.sprite = sprBgOff;
                textVibrateOn.SetActive(false);
                textVibrateOff.SetActive(true);
            }
            else
            {
                toggleVibrate.anchoredPosition = new Vector2(60, 0f);
                bgSetVibrate.sprite = sprBgOn;
                textVibrateOn.SetActive(true);
                textVibrateOff.SetActive(false);
            }

            pnlSetting.SetActive(true);
            popUpSetting.DOPunchScale(Vector3.one * 0.03f, 0.2f, 20, 1).SetUpdate(true);
            BridgeController.instance.ShowBannerCollapsible();
        }
        else
        {
            pnlSetting.SetActive(false);
            BridgeController.instance.HideBannerCollapsible();
        }
    }

    public void SetMusic()
    {
        SoundManager.Instance.PlaySoundButtonClick();
        if (Manager.Instance.SetMusic())
        {
            toggleMusic.DOAnchorPosX(-60, 0.15f).SetEase(Ease.OutBack).SetUpdate(true);
            bgSetMusic.sprite = sprBgOff;
            textMusicOn.SetActive(false);
            textMusicOff.SetActive(true);
        }
        else
        {
            toggleMusic.DOAnchorPosX(60, 0.15f).SetEase(Ease.OutBack).SetUpdate(true);
            bgSetMusic.sprite = sprBgOn;
            textMusicOn.SetActive(true);
            textMusicOff.SetActive(false);
        }
    }

    public void SetSound()
    {
        SoundManager.Instance.PlaySoundButtonClick();
        if (Manager.Instance.SetSound())
        {
            toggleSound.DOAnchorPosX(-60, 0.15f).SetEase(Ease.OutBack).SetUpdate(true);
            bgSetSound.sprite = sprBgOff;
            textSoundOn.SetActive(false);
            textSoundOff.SetActive(true);
        }
        else
        {
            toggleSound.DOAnchorPosX(60, 0.15f).SetEase(Ease.OutBack).SetUpdate(true);
            bgSetSound.sprite = sprBgOn;
            textSoundOn.SetActive(true);
            textSoundOff.SetActive(false);
        }
    }

    public void SetVibrate()
    {
        SoundManager.Instance.PlaySoundButtonClick();
        if (Manager.Instance.SetVibration())
        {
            toggleVibrate.DOAnchorPosX(-60, 0.15f).SetEase(Ease.OutBack).SetUpdate(true);
            bgSetVibrate.sprite = sprBgOff;
            textVibrateOn.SetActive(false);
            textVibrateOff.SetActive(true);
        }
        else
        {
            toggleVibrate.DOAnchorPosX(60, 0.15f).SetEase(Ease.OutBack).SetUpdate(true);
            bgSetVibrate.sprite = sprBgOn;
            textVibrateOn.SetActive(true);
            textVibrateOff.SetActive(false);
        }
    }

    public void OnClickButtonFaceBook()
    {
        SoundManager.Instance.PlaySoundButtonClick();
        BridgeController.instance.ShowFacebook(null);
    }

    public void OnClickButtonTikTok()
    {
        SoundManager.Instance.PlaySoundButtonClick();
        BridgeController.instance.ShowTikTok(null);
    }

    public void OnClickButtonYoutube()
    {
        SoundManager.Instance.PlaySoundButtonClick();
        BridgeController.instance.SubcribeYoutube(null);
    }

    public void OnClose()
    {
        SoundManager.Instance.PlaySoundButtonClick();
        BridgeController.instance.PlayCount++;
        UnityEvent e = new UnityEvent();
        e.AddListener(() =>
        {
            // luồng game sau khi tắt quảng cáo
            DisplayPanelSetting(false);
        });
        UnityEvent eDone = new UnityEvent();
        eDone.AddListener(() =>
        {
            BridgeController.instance.PlayCount = 0;
            AdBreaks.instance.timeElapsedAdBreak = 0;
        });
        BridgeController.instance.ShowInterstitial("setting_close", e,eDone);
    }
}
