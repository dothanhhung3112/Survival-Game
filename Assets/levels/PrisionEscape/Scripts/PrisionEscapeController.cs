using System.Collections.Generic;
using UnityEngine;

public class PrisionEscapeController : MonoBehaviour
{
    public static PrisionEscapeController instance;
    public List<BotPrisionEscape> bots;
    public Material grayMat;
    EnemyPrisionEscape enemy;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Update()
    {
        
    }

    public void RemoveBot(GameObject go)
    {
        for (int i = 0; i < bots.Count; i++)
        {
            if (bots[i].gameObject == go)
            {
                bots.Remove(bots[i]);
            }
        }
    }

    public BotPrisionEscape GetRandomeBot()
    {
        int randomeIndex = Random.Range(0, bots.Count);
        return bots[randomeIndex];
    }
}
