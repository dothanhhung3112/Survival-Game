using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Ddakji : MonoBehaviour
{
    [SerializeField] Rigidbody paperRedRB;
    [SerializeField] Rigidbody paperGreenRB;
    [SerializeField] Transform point;
    [SerializeField] GameObject camDdakji;


    [Header("UI")]
    [SerializeField] Image targetImage;
    [SerializeField] Slider forceSlider;
    [SerializeField] RectTransform forceZone;
    [SerializeField] RectTransform pointCheck;
    [SerializeField] Camera camUI;
    bool isAimRight, isTrueForce, canAim, canForce;
    public bool isWin = false;
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
                CheckIsTrueForce();
                DisplayForceBar(false);
            }

            if (Physics.Raycast(ray, out hit, 20) && canAim)
            {
                if (hit.transform.CompareTag("win"))
                {
                    isAimRight = true;
                }
                canForce = true;
                canAim = false;
                targetTween.Kill();
                targetImage.gameObject.SetActive(false);
                DisplayForceBar(true);
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
            targetImage.rectTransform.position = new Vector3(410f, 0f, 0f);
            targetImage.gameObject.SetActive(false);
        }
    }

    void DisplayForceBar(bool enable)
    {
        if (enable)
        {
            forceSlider.gameObject.SetActive(true);
            forceSlider.DOValue(1, 1.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            forceZone.DOAnchorPosX(-150,2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        }
        else
        {
            forceSlider.value = 0;
            forceZone.anchoredPosition = new Vector2(150,0);
            forceSlider.gameObject.SetActive(false);
        }
    }
     
    void CheckIsTrueForce()
    {
        Vector2 pointCenter = pointCheck.position; // Vì RectTransform có pivot, position chính là tâm

        // Chuyển tâm sang screen space
        Vector2 screenPoint = camUI.WorldToScreenPoint(pointCenter);

        // Kiểm tra tâm có nằm trong forceZone không
        if (RectTransformUtility.RectangleContainsScreenPoint(forceZone, screenPoint, camUI))
        {
            isTrueForce = true;
            paperGreenRB.isKinematic = false;
            paperGreenRB.AddForceAtPosition(Vector3.down * 4f, transform.position, ForceMode.Impulse);
        }
        else
        {
            isTrueForce = false;
            paperGreenRB.isKinematic = false;
            paperGreenRB.AddForceAtPosition(Vector3.down * 2f, transform.position, ForceMode.Impulse);
        }
    }

    public void MakePaperBounce()
    {
        if (isTrueForce && isAimRight)
        {
            paperRedRB.AddForceAtPosition(Vector3.up * 6f, point.transform.position, ForceMode.Impulse);
            isWin = true;
        }
        else
        {
            paperRedRB.AddForceAtPosition(Vector3.up * 2f, point.transform.position, ForceMode.Impulse);
        }
    }

    void StartGame()
    {
        camDdakji.SetActive(true);

    }
}
