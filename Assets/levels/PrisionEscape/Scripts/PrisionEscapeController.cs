using System.Collections.Generic;
using UnityEngine;

public class PrisionEscapeController : MonoBehaviour
{
    public static PrisionEscapeController instance;
    public List<BotPrisionEscape> bots;
    public PlayerPrisionEscape player;
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
        foreach(var item in bots)
        {
            if(item.gameObject == go)
            {
                bots.Remove(item);
            }
        }
    }
}
