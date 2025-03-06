using DG.Tweening;
using Hung;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int cardID;
    bool isFlipped = false;

    private void Start()
    {
        transform.localEulerAngles = new Vector3(180,0,0);
    }

    public void FlipCard()
    {
        if (!isFlipped && MemoryCard.Instance.canClick)
        {
            isFlipped = true;
            transform.DOLocalRotate(new Vector3(0, 0, 0), 0.5f);
            SoundManager.Instance.PlaySoundFlipCard();
            MemoryCard.Instance.CardFlip(this);
        }
    }

    public void HideCard()
    {
        if (isFlipped)
        {
            isFlipped = false;
            transform.DOLocalRotate(new Vector3(180, 0, 0), 0.5f);
        }
    }

    private void OnMouseDown()
    {
        FlipCard();
    }
}
