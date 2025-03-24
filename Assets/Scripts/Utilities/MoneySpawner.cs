using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneySpawner : MonoBehaviour
{
    public static MoneySpawner Instance;

    [SerializeField] GameObject coinPrefab;
    [SerializeField] RectTransform cointParent;
    [SerializeField] RectTransform coinStart;
    [SerializeField] Queue<GameObject> cointObjects = new Queue<GameObject>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            GameObject coinObject = Instantiate(coinPrefab, cointParent);
            coinObject.SetActive(false);
            cointObjects.Enqueue(coinObject);
        }
    }

    public void SpawnCoin(int textStart, int textEnd, TextMeshProUGUI text)
    {
        float coinPerDelay = (0.8f - (0.8f * 0.3f)) / cointObjects.Count;
        int index = 0;
        for (int i = 0; i < cointObjects.Count; i++)
        {
            index++;
            var delay = i * coinPerDelay;
            CoinMove(delay, coinStart.transform.position, text.transform.position);
        }
        text.DOCounter(textStart, textEnd, 0.4f, false).SetEase(Ease.InOutSine).SetDelay(0.6f);
    }

    public void CoinMove(float delay, Vector3 starSpawnPos,Vector3 endPos)
    {
        var coinObject = cointObjects.Dequeue();
        coinObject.SetActive(true);

        var offset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        var starPos = starSpawnPos + offset;
        coinObject.transform.position = starPos;
        coinObject.transform.localScale = new Vector3(.1f, .1f, .1f);
        coinObject.transform.DOScale(Vector3.one, delay);
        coinObject.transform.DOMove(endPos, 0.6f).SetEase(Ease.InBack).SetDelay(delay).OnComplete(() =>
        {
            coinObject.SetActive(false);
        });
        cointObjects.Enqueue(coinObject);
    }
}
