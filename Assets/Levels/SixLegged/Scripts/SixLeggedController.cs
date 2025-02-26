using DG.Tweening;
using UnityEngine;

public class SixLeggedController : MonoBehaviour
{
    public static SixLeggedController Instance;
    [SerializeField] Animator[] animators;
    [SerializeField] float accleration;
    public FlyingStone flyingStone;
    public Ddakji ddakji;
    DOTweenPath path;
    float speed;
    int speedToHash;
    public bool canMove = false;

    public enum MiniGame
    {
        DDakji,Memory,FlyingStone
    }
    public MiniGame minigame;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        path = GetComponent<DOTweenPath>();
        speedToHash = Animator.StringToHash("Speed");
        path.DOPlay();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            canMove = !canMove;
        }

        if (canMove)
        {
            path.DOPlay();
            speed += Time.deltaTime * accleration;
            if (speed > 1) speed = 1;
            foreach (var item in animators)
            {
                item.SetFloat(speedToHash, speed);
            }
        }
        else
        {
            path.DOPause();
            speed -= Time.deltaTime * accleration;
            if (speed <= 0) speed = 0;
            foreach (var item in animators)
            {
                item.SetFloat(speedToHash, speed);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Point"))
        {
            if (!MemoryCard.Instance.isWin)
            {
                canMove = false;
                MemoryCard.Instance.StartGame();
            }else if (!flyingStone.isWin)
            {
                canMove = false;
                flyingStone.StartGame();
            }
        }

    }

    public void Win()
    {

    }

    public void Lose()
    {

    }
}
