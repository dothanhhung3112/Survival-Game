using UnityEngine;

public class Marble : MonoBehaviour
{
    Rigidbody rb;
    public float forceMultiplier = 10f;
    public float angle = 30f;
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private bool isDragging = false;

    [SerializeField] TrailRenderer trail;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPos = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButtonUp(0) && isDragging)
        {
            endTouchPos = Input.mousePosition;
            isDragging = false;

            Vector2 dragVector = endTouchPos - startTouchPos;
            float dragDistance = dragVector.magnitude;
            float forceAmount = dragDistance * forceMultiplier * 0.01f;

            float angleY = Mathf.Atan2(dragVector.x, dragVector.y) * Mathf.Rad2Deg;
            Vector3 throwDirection = Quaternion.Euler(-angle, angleY, 0) * Vector3.forward;
            rb.isKinematic = false;
            Physics.gravity = new Vector3(0, -3f, 0);
            rb.AddForce(throwDirection * forceAmount, ForceMode.Impulse);
        }
    }
}
