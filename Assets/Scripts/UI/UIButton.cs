using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    [SerializeField] protected Button button;
    [SerializeField] protected Image iconImage;
    [SerializeField] protected TextMeshProUGUI buttonText;
    [SerializeField] protected Action customEvent;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Click()
    {
        Press();
    }

    public virtual void Deactivate()
    {
        button.interactable = false;
    }

    public virtual void Activate()
    {
        button.interactable = true;
    }

    public void SetButtonText(string text)
    {
        if(buttonText != null)
        buttonText.text = text;
    }

    public void SetIconImage(Sprite sprite)
    {
        if(iconImage != null)
        {
            iconImage.sprite = sprite;
        }
    }

    public virtual void Press()
    {
        if (button.IsInteractable())
        {
            customEvent?.Invoke();
        }
    }
}
