using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisionEscapeController : MonoBehaviour
{
    public static PrisionEscapeController instance;
    public List<BotPrisionEscape> bots;
    public Material grayMat;
    public PlayerPrisionEscape player;
    public bool isWin = false,gameStarted,isLose = false;
    public float time;

    [Header("EndCard")]
    [SerializeField] Transform[] pathPos;
    [SerializeField] GameObject camWin;
    [SerializeField] DOTweenPath carPath;
    [SerializeField] DOTweenAnimation leftDoor;
    [SerializeField] DOTweenAnimation rightDoor;
    List<Vector3> path = new List<Vector3>();
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < pathPos.Length; i++)
        {
            path.Add(pathPos[i].position);
        }
    }

    private void Update()
    {
        if (!gameStarted) return;
        time -= Time.deltaTime;
        int a = (int)time;
        UIPrisionEscapeController.Instance.UIGamePlay.SetTimeText(a);
    }

    public void StartGame()
    {
        gameStarted = true;
    }

    public BotPrisionEscape GetRandomeBot()
    {
        int randomeIndex = Random.Range(0, bots.Count);
        return bots[randomeIndex];
    }

    public void StartEndCard()
    {
        if (isWin || isLose) return;
        isWin = true;
        camWin.SetActive(true);
        StartCoroutine(EndCard());
    }

    public void Lose()
    {
        if (isWin || isLose) return;
        isLose = true;
        StartCoroutine(Losing());
    }

    IEnumerator Losing()
    {
        player.Die();
        yield return new WaitForSeconds(5f);
        UIPrisionEscapeController.Instance.UIGamePlay.DisplayPanelGameplay(false);
        UIPrisionEscapeController.Instance.UILose.DisplayPanelLose(true);
    }

    IEnumerator EndCard()
    {
        int botCount = 0;
        UIPrisionEscapeController.Instance.UIGamePlay.DisplayPanelGameplay(false);
        player.MoveToCar(path.ToArray());
        yield return new WaitForSeconds(0.2f);
        while (botCount < bots.Count)
        {
            bots[botCount].MoveToCar(path.ToArray());
            botCount++;
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(2f);
        leftDoor.DOPlay();
        rightDoor.DOPlay();
        yield return new WaitForSeconds(1f);
        carPath.DOPlay();
        yield return new WaitForSeconds(3f);
        UIPrisionEscapeController.Instance.UIWin.DisplayPanelWin(true);
    }
}
