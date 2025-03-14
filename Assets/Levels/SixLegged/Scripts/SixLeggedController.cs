using Cinemachine;
using DG.Tweening;
using Hung;
using UnityEngine;

public class SixLeggedController : MonoBehaviour
{
    public static SixLeggedController Instance;

    public FlyingStone flyingStone;
    public Ddakji ddakji;
    public bool canMove = false;
    public float timeMoveCam;
    [SerializeField] Animator[] animators;
    [SerializeField] GameObject camWinLose;
    [SerializeField] float accleration;
    DOTweenPath path;
    public bool isLose = false;
    float speed,elapsedTime;
    int speedToHash;

    public enum MiniGame
    {
        DDakji, Memory, FlyingStone
    }
    public MiniGame minigame;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        timeMoveCam = Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.BlendTime;
    }

    private void Start()
    {
        SoundManager.Instance.PlayBGMusic6();
        path = GetComponent<DOTweenPath>();
        speedToHash = Animator.StringToHash("Speed");
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
            elapsedTime += Time.deltaTime;
            if (elapsedTime > 0.4f)
            {
                SoundManager.Instance.PlaySoundWalk();
                elapsedTime = 0;
            }
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
                UISixLeggedController.Instance.UIGamePlay.DisplayPanelGameplay(true);
                canMove = false;
                MemoryCard.Instance.StartGame();
                minigame = MiniGame.Memory;
            }
            else if (!flyingStone.isWin)
            {
                UISixLeggedController.Instance.UIGamePlay.DisplayPanelGameplay(true);
                canMove = false;
                flyingStone.StartGame();
                minigame = MiniGame.FlyingStone;
            }
            else if (!ddakji.isWin)
            {
                UISixLeggedController.Instance.UIGamePlay.DisplayPanelGameplay(true);
                canMove = false;
                ddakji.StartGame();
                minigame = MiniGame.DDakji;
            }
        }

        if (other.CompareTag("win"))
        {
            Win();
        }
    }

    public void Win()
    {
        canMove = false;
        camWinLose.SetActive(true);
        SoundManager.Instance.StopMusic();
        SoundManager.Instance.PlaySoundWin();
        DOVirtual.DelayedCall(timeMoveCam, delegate
        {
            foreach (var item in animators)
            {
                int randomDance = Random.Range(1, 4);
                item.Play("Dance" + randomDance);
            }
        });
        DOVirtual.DelayedCall(5f, delegate
        {
            UISixLeggedController.Instance.UIWin.DisplayPanelWin(true);
        });
    }

    public void Lose()
    {
        isLose = true;
        camWinLose.SetActive(true);
        SoundManager.Instance.StopMusic();
        SoundManager.Instance.PlaySoundLose();
        DOVirtual.DelayedCall(timeMoveCam, delegate
        {
            foreach (var item in animators)
            {
                ObjectPooler.instance.SetObject("bloodEffect", item.transform.position + new Vector3(0, 0.5f, 0));
                int randomDie = Random.Range(2, 4);
                item.Play("die" + randomDie);
                transform.position += new Vector3(0, 0.02f, 0);
            }
            SoundManager.Instance.PlaySoundGunShooting();
            SoundManager.Instance.PlaySoundMaleHited();
        });
        DOVirtual.DelayedCall(5f, delegate
        {
            UISixLeggedController.Instance.UIGamePlay.DisplayPanelGameplay(false);
            UISixLeggedController.Instance.UILose.DisplayPanelLose(true);
        });
    }
}
