using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Ddakji : MonoBehaviour
{
    [SerializeField] Rigidbody paperRedRB;
    [SerializeField] Rigidbody paperGreenRB;
    [SerializeField] Transform point;
    [SerializeField] Image targetImage;
    [SerializeField] Camera camUI;
    bool isAimRight,isTrueForce,canAim,canForce;
    Tween targetTween;
    private void Start()
    {
        DisplayArrow(true);
    }

    private void Update()
    {
        Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(camUI, targetImage.rectTransform.position);
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (canForce)
            {
                canForce = false;
            }

            if (Physics.Raycast(ray,out hit,20) && canAim)
            {
                if (hit.transform.CompareTag("win"))
                {
                    isAimRight=true;
                }
                canForce = true;
                canAim = false;
                targetTween.Kill();
                targetImage.gameObject.SetActive(false);
            }
        }
    }

    void DisplayArrow(bool enable)
    {
        if (enable)
        {
            targetImage.gameObject.SetActive(true);
            canAim = true;
            targetTween = targetImage.rectTransform.DOAnchorPosX(-410, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
        else
        {
            targetTween.Kill();
            targetImage.rectTransform.position = new Vector3(410f, 0f,0f);
            targetImage.gameObject.SetActive(false);
        }
    }

    void ThrowPaper()
    {

        paperRedRB.AddForceAtPosition(Vector3.up * 5f, point.transform.position, ForceMode.Impulse);
    }
}
