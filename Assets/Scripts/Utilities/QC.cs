#if UNITY_EDITOR || UNITY_STANDALONE

using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Hung;

namespace _Scripts.Extension
{
    public class QC : MonoBehaviour
    {
        public static QC instance;
        public GameObject UI;
        public bool isAllowKeyCode = false;

        [SerializeField] Transform parentDifLevel;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else Destroy(this.gameObject);
        }

        private void Update()
        {
            if (isAllowKeyCode) return;
            if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.F))
            {
                isAllowKeyCode = true;
                Screen.SetResolution(1920, 1080, true);
                DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
                return;
            }
            if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.G))
            {
                isAllowKeyCode = true;
                Screen.SetResolution(1080, 1920, true);
                DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
                return;
            }
            if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.V))
            {
                isAllowKeyCode = true;
                Screen.SetResolution(1080, 1080, true);
                DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
                return;
            }
            if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.N))
            {
                isAllowKeyCode = true;
                Screen.SetResolution(1350, 1080, true);
                DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
                return;
            }
            if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.B))
            {
                isAllowKeyCode = true;
                Screen.SetResolution(1080, 1350, true);
                DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
                return;
            }
            if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.F))
            {
                isAllowKeyCode = true;
                Screen.SetResolution(1920, 1080, false);
                DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
                return;
            }
            if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.G))
            {
                isAllowKeyCode = true;
                Screen.SetResolution(1080, 1920, false);
                DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
                return;
            }
            if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.V))
            {
                isAllowKeyCode = true;
                Screen.SetResolution(1080, 1080, false);
                DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
                return;
            }
            if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.N))
            {
                isAllowKeyCode = true;
                Screen.SetResolution(1350, 1080, false);
                DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
                return;
            }
            if (!isAllowKeyCode && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKey(KeyCode.B))
            {
                isAllowKeyCode = true;
                Screen.SetResolution(1080, 1350, false);
                DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
                return;
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
            }

            if (Input.GetKeyDown(KeyCode.R))
            {

            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
            {
                Manager.Instance.levelWinList.Clear();
                Manager.Instance.levelLoseList.Clear();
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.W))
            {

            }

            if (!isAllowKeyCode && Input.GetKeyDown(KeyCode.LeftArrow)) //back
            {
                isAllowKeyCode = true;
                if (Manager.Instance.CurrentLevel > 0)
                {
                    Manager.Instance.CurrentLevel--;
                    if (Manager.Instance.CurrentLevel < 1) Manager.Instance.CurrentLevel = 1;
                }
                SceneManager.LoadScene(Manager.Instance.CurrentLevel);
                DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
            }

            if (!isAllowKeyCode && Input.GetKeyDown(KeyCode.RightArrow)) //next
            {
                isAllowKeyCode = true;
                Manager.Instance.LoadNextLevel(true);
                DOVirtual.DelayedCall(0.5f, delegate { isAllowKeyCode = false; });
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Manager.Instance.IsMuteSound = !Manager.Instance.IsMuteSound;
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                Debug.Log(2 % 9);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
            }
        }
    }
}
#endif