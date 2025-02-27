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
        if (SixLeggedController.Instance.minigame == SixLeggedController.MiniGame.FlyingStone)
        {
            rb.isKinematic = false;
            Vector3 direction = (collision.transform.position - transform.position).normalized;
            rb.AddForce(direction * 3f, ForceMode.Impulse);
            flyingStone.Win();
        }
        else if (SixLeggedController.Instance.minigame == SixLeggedController.MiniGame.DDakji && !SixLeggedController.Instance.ddakji.isWin)
        {
            SixLeggedController.Instance.ddakji.MakePaperBounce();
            SixLeggedController.Instance.ddakji.StartResetPaper();
        }
    }
}
