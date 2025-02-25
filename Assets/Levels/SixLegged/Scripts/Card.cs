using DG.Tweening;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int cardID;
    bool isFlipped = false;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(180, 0, 0);
    }

    public void FlipCard()
    {
        if (!isFlipped && MemoryCard.Instance.canClick)
        {
            isFlipped = true;
            transform.DORotate(new Vector3(0, 0, 0), 0.5f);
            MemoryCard.Instance.CardFlip(this);
        }
    }

    public void HideCard()
    {
        if (isFlipped)
        {
            isFlipped = false;
            transform.DORotate(new Vector3(180, 0, 0), 0.5f);
        }
    }

    private void OnMouseDown()
    {
        FlipCard();
    }
}
