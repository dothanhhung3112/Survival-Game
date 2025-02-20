using UnityEngine;

namespace Hung.Gameplay.Marble
{
    public class Marble : MonoBehaviour
    {
        Rigidbody rb;
        public float forceMultiplier = 10f;
        public float angle = 30f;
        public bool isEnemyMarble = false;
        [SerializeField] TrailRenderer trail;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Shoot(Vector3 force)
        {
            rb.isKinematic = false;
            Physics.gravity = new Vector3(0, -3f, 0);
            rb.AddForce(force, ForceMode.Impulse);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Finish"))
            {
                MarbleGameController.Instance.IncreaseMarbleHoled(isEnemyMarble);
                MarbleGameController.Instance.CheckEndGame();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            trail.enabled = false;
        }
    }
}
