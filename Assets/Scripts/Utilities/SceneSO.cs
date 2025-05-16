using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneSO", menuName = "SceneSO")]
public class SceneSO : ScriptableObject
{
    public List<string> scenes = new List<string>();
    public string sceneName;
    public int sceneIndex;
    public AudioClip backgroundMusic;
    public Minigame minigameType;
    public float Timelimit;
}


