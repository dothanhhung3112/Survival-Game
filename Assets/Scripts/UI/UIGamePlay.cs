using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hung.UI
{
    public class UIGamePlay : MonoBehaviour
    {
        public enum GamePlay { Marble, Normal, GameFight, GreenRedLight }
        [SerializeField] public GamePlay gamePlay;
        [SerializeField] GameObject gameplayPanel;

        [SerializeField] GameObject greenImage;
        [SerializeField] Image redImage;

        [SerializeField] TextMeshProUGUI timeText;
        [SerializeField] TextMeshProUGUI botLeftText;

        [SerializeField] Image[] imagePlayerMarbles;
        [SerializeField] Image[] imageEnemyMarbles;
        [SerializeField] TextMeshProUGUI textPlayerMarble;
        [SerializeField] TextMeshProUGUI textEnemyMarble;

        public void DisplayPanelGameplay(bool enable)
        {
            if (enable)
            {
                NativeAdsController.Instance?.DisplayNativeAdsInGame(true);
                gameplayPanel.SetActive(true);
            }
            else
            {
                NativeAdsController.Instance?.DisplayNativeAdsInGame(false);
                gameplayPanel.SetActive(false);
            }
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

        public void SetBotLeftText(float botLeft, float botSum)
        {
            botLeftText.text = $"{botLeft}/{botSum}";
        }

        public void UpdatePlayerMarbleUI(int playerMarble)
        {
            for (int i = 0; i < 3; i++)
            {
                if (i < (3 - playerMarble))
                {
                    imagePlayerMarbles[i].color = new Color32(150, 150, 150, 150);
                }
                else
                {
                    imagePlayerMarbles[i].color = new Color32(255, 255, 255, 255);
                }
            }
        }

        public void UpdateEnemyMarbleUI(int enemyMarble)
        {
            for (int i = 0; i < (3 - enemyMarble); i++)
            {
                imageEnemyMarbles[i].color = new Color32(150, 150, 150, 150);
            }
        }

        public void UpdateTextPlayerMarble(int value)
        {
            textPlayerMarble.text = $"{value}";
        }

        public void UpdateTextEnemyMarble(int value)
        {
            textEnemyMarble.text = $"{value}";
        }
    }
}
