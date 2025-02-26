using UnityEngine;

public class Target : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] FlyingStone flyingStone;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = false;
        Vector3 direction = (collision.transform.position - transform.position).normalized;
        rb.AddForce(direction * 3f,ForceMode.Impulse);
        flyingStone.Win();
    }
}
