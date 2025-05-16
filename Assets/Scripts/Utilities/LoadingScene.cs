using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Hung;

namespace Duc
{
    public class LoadingScene : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtLoading;
        [SerializeField] private TextMeshProUGUI txtProgress;
        [SerializeField] private Slider progress;

        private void Start()
        {
            SceneLoader.Instance.LoadSceneByIndex(Manager.Instance.CurrentLevel);
        }

        public void LoadLevel(int scene)
        {
            if (txtLoading != null)
            {
                CoroutineTextLoading = StartCoroutine(TextLoading());
            }
            StartCoroutine(LoadAsyncGame(scene));
        }

        Coroutine CoroutineTextLoading;
        IEnumerator TextLoading()
        {
            for (; ;)
            {
                txtLoading.text = "Loading   ";
                yield return new WaitForSeconds(0.1f);
                txtLoading.text = "Loading.  ";
                yield return new WaitForSeconds(0.1f);
                txtLoading.text = "Loading.. ";
                yield return new WaitForSeconds(0.1f);
                txtLoading.text = "Loading...";
                yield return new WaitForSeconds(0.1f);
            }
        }

        IEnumerator LoadAsyncGame(int scene)
        {
            if (txtProgress != null) txtProgress.text = "0%";
            if (progress != null) progress.value = 0f;

            yield return new WaitForSeconds(0.3f);
            int fakeStep = UnityEngine.Random.Range(0, 10000) % 20 + 60;
            yield return new WaitForSeconds(3.2f - 0.04f * fakeStep);
            for (int i = 0; i < fakeStep; i++)
            {
                yield return new WaitForSeconds(0.03f);
                if (txtProgress != null) txtProgress.text = "" + i + "%";
                if (progress != null) progress.value = i * 0.01f;
            }
            yield return new WaitForSeconds(0.5f);
            AsyncOperation async = SceneManager.LoadSceneAsync(scene);
            while (!async.isDone)
            {
                int process = Mathf.RoundToInt(60 * (async.progress / 0.9f)) + fakeStep;
                if (txtProgress != null) txtProgress.text = Mathf.Min(100, process) + "%";
                if (progress != null) progress.value = Mathf.Min(1, process / 100f);
                yield return null;
            }
        }
    }
}