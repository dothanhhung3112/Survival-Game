using UnityEngine;

public class UIBase : MonoBehaviour
{
    [SerializeField] GameObject uiPanel;
    public virtual void Show()
    {
        uiPanel.SetActive(true);
    }

    public virtual void Hide()
    {
        uiPanel.SetActive(false);
    }

    public virtual void SetData()
    {

    }
}
