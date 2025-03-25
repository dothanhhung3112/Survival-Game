using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Point : MonoBehaviour
{
    public enum PointState { Lose, Win, Current, Lock }
    [SerializeField] Image circle;
    [SerializeField] GameObject lockIcon;
    [SerializeField] GameObject loseIcon;
    [SerializeField] GameObject winIcon;
    [SerializeField] TextMeshProUGUI levelText;

    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite currentSprite;
    public void SetData(PointState pointState,int level = 0)
    {
        ResetPointState();
        switch (pointState)
        {
            case PointState.Lose:
                loseIcon.SetActive(true);
                break;
            case PointState.Win:
                winIcon.SetActive(true);
                break;
            case PointState.Current:
                circle.sprite = currentSprite;
                levelText.gameObject.SetActive(true);
                levelText.text = $"{level}";
                break;
            case PointState.Lock:
                lockIcon.SetActive(true); 
                break;
        }
    }

    void ResetPointState()
    {
        circle.sprite = normalSprite;

        if (lockIcon.activeSelf) 
        lockIcon.SetActive(false);

        if(loseIcon.activeSelf)
            loseIcon.SetActive(false);

        if (winIcon.activeSelf)
            winIcon.SetActive(false);

        if (levelText.gameObject.activeSelf)
            levelText.gameObject.SetActive(false);
    }
}
