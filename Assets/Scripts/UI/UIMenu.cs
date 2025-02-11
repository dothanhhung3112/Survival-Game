using UnityEngine;

public class UIMenu : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;

    public void DisplayPanelMenu(bool enable)
    {
        if (enable)
        {
            menuPanel.SetActive(true);
        }
        else
        {
            menuPanel.SetActive(false);
        }
    }
}
