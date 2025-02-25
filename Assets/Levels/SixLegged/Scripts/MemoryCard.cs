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
    [HideInInspector] public Card firstCard;
    [HideInInspector] public Card secondCard;
    public bool canClick;
    int pairMatched = 0;

    private void Awake()
    {
        if(Instance == null)
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
        time -= Time.deltaTime;
        int a = (int)time;
        //Update ui text

    }

    void CreateCard()
    {

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

    public void StartMemoryCard()
    {

    }
}
