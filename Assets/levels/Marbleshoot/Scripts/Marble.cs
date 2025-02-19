using UnityEngine;

public class Marble : MonoBehaviour
{
    Rigidbody rb;
    public float forceMultiplier = 10f; 
    public float angle = 30f; 
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private bool isDragging = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            startTouchPos = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButtonUp(0) && isDragging) // Khi thả ngón tay
        {
            endTouchPos = Input.mousePosition;
            isDragging = false;

            Vector2 dragVector = endTouchPos - startTouchPos; // Lấy hướng vuốt
            float dragDistance = dragVector.magnitude; // Khoảng cách vuốt
            float forceAmount = dragDistance * forceMultiplier * 0.01f; // Chuyển khoảng cách thành lực

            // Chỉ lấy lực theo trục Z và Y (bỏ X để không lệch hướng)
            Vector3 throwDirection = Quaternion.Euler(-angle, 0, 0) * Vector3.forward; // Ném theo góc chéo
            rb.AddForce(throwDirection * forceAmount, ForceMode.Impulse); // Bắn object
        }
    }
}
