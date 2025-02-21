#if UNITY_EDITOR || UNITY_STANDALONE
using UnityEditor;
using Hung.UI;

[CustomEditor(typeof(UIGamePlay))]
public class UIGamePlayEdit : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        UIGamePlay uiGameplay = (UIGamePlay)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("gamePlay"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("gameplayPanel"));
        if (uiGameplay.gamePlay == UIGamePlay.GamePlay.Marble)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("imagePlayerMarbles"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("imageEnemyMarbles"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("textPlayerMarble"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("textEnemyMarble"));
        }
        else if (uiGameplay.gamePlay == UIGamePlay.GamePlay.GreenRedLight)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("greenImage"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("redImage"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("timeText"));
        }
        else if (uiGameplay.gamePlay == UIGamePlay.GamePlay.GameFight)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("timeText"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("botLeftText"));
        }
        else if (uiGameplay.gamePlay == UIGamePlay.GamePlay.Normal)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("timeText"));
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif