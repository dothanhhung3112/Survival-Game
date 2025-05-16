using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Point : MonoBehaviour
{
    public enum PointState { Lose, Win, Current, Lock }
    [SerializeField] GameObject winPoint;
    [SerializeField] GameObject losePoint;
    [SerializeField] GameObject currentPoint;
    [SerializeField] GameObject lockPoint;
    [SerializeField] TextMeshProUGUI levelText;

    public void SetData(PointState pointState,int level = 0)
    {
        ResetPointState();
        switch (pointState)
        {
            case PointState.Lose:
                losePoint.SetActive(true);
                break;
            case PointState.Win:
                winPoint.SetActive(true);
                break;
            case PointState.Current:
                currentPoint.SetActive(true);
                levelText.gameObject.SetActive(true);
                levelText.text = $"{level}";
                break;
            case PointState.Lock:
                lockPoint.SetActive(true); 
                break;
        }
    }

    void ResetPointState()
    {
        if (lockPoint.activeSelf)
            lockPoint.SetActive(false);

        if(winPoint.activeSelf)
            winPoint.SetActive(false);

        if (losePoint.activeSelf)
            losePoint.SetActive(false);

        if (currentPoint.activeSelf)
            currentPoint.SetActive(false);

    }
}
