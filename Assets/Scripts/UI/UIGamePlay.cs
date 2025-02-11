using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlay : MonoBehaviour
{
    [SerializeField] GameObject gameplayPanel;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Image redImage;
    [SerializeField] GameObject greenImage;

    public void DisplayPanelGameplay(bool enable)
    {
        if (enable)
        {
            gameplayPanel.SetActive(true);
        }
        else
        {
            gameplayPanel.SetActive(false);
        }
    }

    private void Update()
    {
        
    }

    public void SetRedImageFillAmount(float value)
    {
        redImage.fillAmount += value;
    }

    public void TurnOnGreenImage()
    {
        greenImage.SetActive(true);
        redImage.fillAmount = 0;
    }

    public void SetTimeText(float value)
    {
        timeText.text = $"{value}";
    }
}
