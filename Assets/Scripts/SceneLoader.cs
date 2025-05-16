using Hung;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] List<SceneSO> _sceneLists = new List<SceneSO>();
    public List<SceneSO> sceneLists => _sceneLists;

    [Header("---------------loadingUI---------------")]
    [SerializeField] GameObject loadingPanel;
    [SerializeField] private Slider progress;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        LoadSceneByIndex(Manager.Instance.CurrentLevel);
    }

    public void LoadSceneByIndex(int index)
    {
        StartCoroutine(LoadSceneAsync(index));
    }
    IEnumerator LoadSceneAsync(int index, Action sceneLoadCompleted = null)
    {
        loadingPanel.SetActive(true);
        progress.value = 0;

        AsyncOperation operation = SceneManager.LoadSceneAsync(GetSceneNameByIndex(index));
        operation.allowSceneActivation = false;

        float elapsedTime = 0;
        while (operation.progress < 0.9f || elapsedTime < 3)
        {
            elapsedTime += Time.deltaTime;
            if (progress != null) progress.value = Mathf.Clamp01(elapsedTime / 3);
            yield return null;
        }
        operation.allowSceneActivation = true;
        loadingPanel.SetActive(false);
    }

    string GetSceneNameByIndex(int index)
    {
        foreach (var item in sceneLists)
        {
            if (index == item.sceneIndex)
            {
                return item.name;
            }
        }
        return null;
    }

    private void OnDrawGizmosSelected()
    {
        _sceneLists = Resources.LoadAll<SceneSO>("SceneSO").ToList();
    }
}
