using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Ddakji : MonoBehaviour
{
    [SerializeField] Rigidbody paperRedRB;
    [SerializeField] Rigidbody paperGreenRB;
    [SerializeField] Transform point;
    [SerializeField] GameObject camDdakji;
    [SerializeField] Transform paperRedPos;
    [SerializeField] Transform paperGreenPos;
    [SerializeField] float time;

    [Header("UI")]
    [SerializeField] Image targetImage;
    [SerializeField] Slider forceSlider;
    [SerializeField] RectTransform forceZone;
    [SerializeField] RectTransform pointCheck;
    [SerializeField] Camera camUI;
    public bool isAimRight, isTrueForce, canAim, canForce, canCountTime;
    public bool isWin = false;
    Tween targetTween,zoneTween,sliderTween;

    private void Update()
    {
        if (isWin || !canCountTime || SixLeggedController.Instance.isLose) return;
        time -= Time.deltaTime;
        int a = (int)time;
        UISixLeggedController.Instance.UIGamePlay.SetTimeText(a);
        if (a <= 0)
        {
            SixLeggedController.Instance.Lose();
        }
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(camUI, targetImage.rectTransform.position);
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0))
        {
            if (canForce)
            {
                CheckIsTrueForce();
                DisplayForceBar(false);
            }

            if (Physics.Raycast(ray, out hit, 20) && canAim)
            {
                if (hit.transform.CompareTag("win"))
                {
                    isAimRight = true;
                }
                DisplayArrow(false);
                DisplayForceBar(true);
            }
        }
    }

    void DisplayArrow(bool enable)
    {
        if (enable)
        {
            canAim = true;
            targetImage.gameObject.SetActive(true);
            if (targetTween == null || !targetTween.IsActive())
            {
                targetTween = targetImage.rectTransform.DOAnchorPosX(-410, 1f)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine);
            }
        }
        else
        {
            canAim = false;
            targetTween.Kill(true);
            targetImage.rectTransform.anchoredPosition = new Vector2(410,0);
            targetImage.gameObject.SetActive(false);
        }
    }

    void DisplayForceBar(bool enable)
    {
        if (enable)
        {
            canForce = true;
            forceSlider.gameObject.SetActive(true);
            sliderTween = forceSlider.DOValue(1, 1.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            zoneTween = forceZone.DOAnchorPosY(-137,2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }
        else
        {
            canForce = false;
            sliderTween.Kill();
            zoneTween.Kill();
            zoneTween = null;
            sliderTween = null;
            forceSlider.value = 0;
            forceZone.anchoredPosition = new Vector2(0,137);
            forceSlider.gameObject.SetActive(false);
        }
    }
     
    void CheckIsTrueForce()
    {
        Vector2 pointCenter = pointCheck.position; 
        Vector2 screenPoint = camUI.WorldToScreenPoint(pointCenter);
        if (RectTransformUtility.RectangleContainsScreenPoint(forceZone, screenPoint, camUI))
        {
            isTrueForce = true;
            paperGreenRB.isKinematic = false;
            paperGreenRB.AddForce(Vector3.down * 4, ForceMode.Impulse);
        }
        else
        {
            isTrueForce = false;
            paperGreenRB.isKinematic = false;
            paperGreenRB.AddForce(Vector3.down * 2, ForceMode.Impulse);
        }
    }

    public void StartResetPaper()
    {
        StartCoroutine(ResetPaper());
    }

    IEnumerator ResetPaper()
    {
        yield return new WaitForSeconds(2f);
        if(isWin) yield break;
        paperRedRB.transform.position = paperRedPos.position;
        paperRedRB.transform.rotation = Quaternion.identity;
        paperGreenRB.transform.position = paperGreenPos.position;
        paperGreenRB.transform.rotation = Quaternion.identity;
        paperGreenRB.isKinematic = true;
        DisplayArrow(true);
    }

    public void MakePaperBounce()
    {
        if (isTrueForce && isAimRight)
        {
            paperRedRB.AddForceAtPosition(Vector3.up * 4f, point.transform.position, ForceMode.Impulse);
            Win();
        }
        else
        {
            paperRedRB.AddForceAtPosition(Vector3.up, point.transform.position, ForceMode.Impulse);
        }
    }

    public void Win()
    {
        isWin = true;
        UISixLeggedController.Instance.UIGamePlay.DisplayPanelGameplay(false);
        DOVirtual.DelayedCall(0.5f, delegate
        {
            camDdakji.SetActive(false);
            DOVirtual.DelayedCall(SixLeggedController.Instance.timeMoveCam, delegate
            {
                SixLeggedController.Instance.canMove = true;
                paperRedRB.gameObject.SetActive(false);
                paperGreenRB.gameObject.SetActive(false);
            });
        });
    }

    public void StartGame()
    {
        camDdakji.SetActive(true);
        DOVirtual.DelayedCall(SixLeggedController.Instance.timeMoveCam, delegate
        {
            canCountTime = true;
            DisplayArrow(true);
            paperGreenRB.gameObject.SetActive(true);
        });
    }
}
