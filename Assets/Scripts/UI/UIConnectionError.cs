using DG.Tweening;
using Hung;
using System.Collections;
using UnityEngine;
using TA;

namespace _Scripts.UI
{
    public class UIConnectionError : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject pnlConnectionError;
        [SerializeField] private Transform dialog;

        IEnumerator Start()
        {
            yield return new WaitUntil(() => ConnectInternetManager.instance != null);

            ConnectInternetManager.instance.onConnectInternet += () =>
            {
                DisplayPanelConnectionError(false);
            };
            ConnectInternetManager.instance.onDisconnectInternet += () =>
            {
                DisplayPanelConnectionError(true);
            };
        }

        public void DisplayPanelConnectionError(bool enable)
        {
            if (enable)
            {
                if (ACEPlay.Bridge.BridgeController.instance.InternetRequire && Manager.Instance.Level >= ACEPlay.Bridge.BridgeController.instance.LevelCheckInternet)
                {
                    pnlConnectionError.SetActive(true);
                    dialog.DOPunchScale(Vector3.one * 0.03f, 0.2f, 20, 1);
                }
            }
            else
            {
                pnlConnectionError.SetActive(false);
            }
        }

        public void OnClickButtonTryAgain()
        {
            ConnectInternetManager.instance.Check();
        }
    }
}