using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MemoryCard : MonoBehaviour
{
    public static MemoryCard Instance;

    [SerializeField] List<Card> cards;
    [SerializeField] Transform[] cardPos;
    [SerializeField] float time;
    [SerializeField] GameObject cam;
    [SerializeField] Transform table;
    Card firstCard;
    Card secondCard;
    public bool isWin = false;
    public bool canClick;
    int pairMatched = 0;
    bool canCountTime = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        cards = cards.OrderBy(x => Random.value).ToList();
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.position = cardPos[i].position;
        }
        canClick = true;
    }

    private void Update()
    {
        if (isWin || !canCountTime ||SixLeggedController.Instance.isLose) return;
        time -= Time.deltaTime;
        int a = (int)time;
        UISixLeggedController.Instance.UIGamePlay.SetTimeText(a);
        if (a <= 0)
        {
            SixLeggedController.Instance.Lose();
        }
    }

    public void StartGame()
    {
        cam.SetActive(true);
        DOVirtual.DelayedCall(SixLeggedController.Instance.timeMoveCam, delegate
        {
            canCountTime = true;
        });
    }

    public void CardFlip(Card card)
    {
        if (firstCard == null)
        {
            firstCard = card;
        }
        else if (secondCard == null)
        {
            secondCard = card;
            CheckPair();
        }
    }

    void CheckPair()
    {
        if (firstCard.cardID == secondCard.cardID)
        {
            pairMatched++;
            firstCard = null;
            secondCard = null;
        }
        else
        {
            StartCoroutine(FlipBackCard());
        }

        if (pairMatched == 3)
        {
            isWin = true;
            UISixLeggedController.Instance.UIGamePlay.DisplayPanelGameplay(false);
            DOVirtual.DelayedCall(0.5f, delegate
            {
                table.DOLocalMoveX(5, 1.5f);
                cam.SetActive(false);
                DOVirtual.DelayedCall(SixLeggedController.Instance.timeMoveCam, delegate
                {
                    SixLeggedController.Instance.canMove = true;
                });
            });
        }
    }

    IEnumerator FlipBackCard()
    {
        canClick = false;
        yield return new WaitForSeconds(1f);
        firstCard.HideCard();
        secondCard.HideCard();
        firstCard = null;
        secondCard = null;
        yield return new WaitForSeconds(0.5f);
        canClick = true;
    }
}
