using UnityEngine;

public class FaceToCam : MonoBehaviour
{
    [SerializeField] Transform camTransform;

    private void LateUpdate()
    {
        transform.LookAt(camTransform);
    }
}
