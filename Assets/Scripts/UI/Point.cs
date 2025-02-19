using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Point : MonoBehaviour
{
    public enum PointState { Lose, Win, Current, Lock }
    [SerializeField] Image circle;
    [SerializeField] GameObject lockIcon;
    [SerializeField] GameObject loseIcon;
    [SerializeField] TextMeshProUGUI levelText;

    [SerializeField] Color32 colorLose;
    [SerializeField] Color32 colorWin;
    [SerializeField] Color32 colorCurrent;
    [SerializeField] Color32 colorLock;
    public void SetData(PointState pointState,int level = 0)
    {
        switch (pointState)
        {
            case PointState.Lose:
                circle.color = colorLose;
                SetInactiveChilds();
                loseIcon.SetActive(true);
                break;
            case PointState.Win:
                circle.color = colorWin;
                SetInactiveChilds();
                levelText.gameObject.SetActive(true);
                levelText.text = $"{level}";
                break;
            case PointState.Current:
                circle.color = colorCurrent;
                SetInactiveChilds();
                levelText.gameObject.SetActive(true);
                levelText.text = $"{level}";
                break;
            case PointState.Lock:
                circle.color = colorLock;
                SetInactiveChilds();
                lockIcon.SetActive(true); 
                break;
        }
    }

    void SetInactiveChilds()
    {
        if(lockIcon.activeSelf) 
        lockIcon.SetActive(false);

        if(loseIcon.activeSelf)
            loseIcon.SetActive(false);

        if(levelText.gameObject.activeSelf)
            levelText.gameObject.SetActive(false);
    }

    void Update()
    {

    }
}
