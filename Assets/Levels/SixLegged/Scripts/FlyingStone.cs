using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlyingStone : MonoBehaviour
{
    public bool isWin;
    [SerializeField] GameObject camFlyingStone;
    [SerializeField] Image arrow;
    [SerializeField] Image arrowFill;
    [SerializeField] Rigidbody stone;
    [SerializeField] Transform target;
    [SerializeField] float time;
    bool isChangeDirection, isChangeForce,canCountTime;
    float forceAmount;
    Tween arrowTween, fillArrowTween;
    Vector3 stonePos;
    Vector3 throwDirection;

    private void Start()
    {
        stonePos = stone.transform.position;
        stone.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isWin || !canCountTime || SixLeggedController.Instance.isLose) return;
        time -= Time.deltaTime;
        int a = (int)time;
        UISixLeggedController.Instance.UIGamePlay.SetTimeText(a);
        if (a <= 0)
        {
            canCountTime = false;
            SixLeggedController.Instance.Lose();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (isChangeForce)
            {
                isChangeForce = false;
                forceAmount = 20 * arrowFill.fillAmount;
                ThrowStone();
            }

            if (isChangeDirection)
            {
                isChangeDirection = false;
                isChangeForce = true;
                arrowTween.Kill();
                Vector3 direction = (target.position - stone.transform.position).normalized;
                throwDirection = Quaternion.Euler(0, -arrow.transform.rotation.eulerAngles.z, 0) * direction;
                fillArrowTween = arrowFill.DOFillAmount(1, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            }
        }
    }

    void ThrowStone()
    {
        stone.isKinematic = false;
        stone.AddForce(throwDirection * forceAmount, ForceMode.Impulse);
        DisplayArrow(false);
        StartCoroutine(RestartStone());
    }

    IEnumerator RestartStone()
    {
        yield return new WaitForSeconds(4f);
        if (isWin) yield break;
        stone.isKinematic = true;
        stone.transform.position = stonePos;
        DisplayArrow(true);
    }

    void DisplayArrow(bool enable)
    {
        if (enable)
        {
            //reset arrow
            fillArrowTween?.Kill();
            arrowFill.fillAmount = 0;
            arrow.rectTransform.localEulerAngles = new Vector3(0, 0, -50);

            arrow.gameObject.SetActive(true);
            arrowTween = arrow.rectTransform.DOLocalRotate(new Vector3(0, 0, 50), 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            isChangeDirection = true;
        }
        else
        {
            arrow.gameObject.SetActive(false);
        }
    }

    public void Revive()
    {
        //stone
        arrowTween.Kill();
        fillArrowTween?.Kill();
        time += 20;
        isChangeDirection = false;
        isChangeForce = false;
        stone.isKinematic = true;
        stone.transform.position = stonePos;
        StartGame();
    }

    public void Win()
    {
        isWin = true;
        DisplayArrow(false);
        stone.gameObject.SetActive(false);
        UISixLeggedController.Instance.UIGamePlay.DisplayPanelGameplay(false);
        DOVirtual.DelayedCall(0.5f, delegate
        {
            camFlyingStone.SetActive(false);
            DOVirtual.DelayedCall(SixLeggedController.Instance.timeMoveCam, delegate
            {
                SixLeggedController.Instance.canMove = true;
            });
        });
    }

    public void StartGame()
    {
        camFlyingStone.SetActive(true);
        DOVirtual.DelayedCall(SixLeggedController.Instance.timeMoveCam, delegate
        {
            canCountTime = true;
            DisplayArrow(true);
            stone.gameObject.SetActive(true);
            UISixLeggedController.Instance.UIGamePlay.DisplayPanelGameplay(true);
        });
    }
}
