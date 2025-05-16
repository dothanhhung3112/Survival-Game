using UnityEngine;
using Hung.Gameplay.GreenRedLight;

namespace Hung.Tools
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Transform visualTransform;

        private Transform hitTransform;
        private bool isEnemyShot;
        private float shootingForce;
        private Vector3 direction;
        private Vector3 hitPoint;

        public void Launch(float shootingForce, Transform hitTransform, Vector3 hitPoint)
        {
            direction = (hitPoint - transform.position).normalized;
            isEnemyShot = false;
            this.hitTransform = hitTransform;
            this.shootingForce = shootingForce;
            this.hitPoint = hitPoint;
        }

        private void Update()
        {
            Move();
            Rotate();
            //CheckDistanceToEnemy();
            CheckDistanceToPlayer();
        }

        private void Move()
        {
            transform.Translate(direction * shootingForce * Time.deltaTime, Space.World);
        }

        private void CheckDistanceToPlayer()
        {
            float distance = Vector3.Distance(transform.position, hitPoint);
            if (distance <= 0.2f && !isEnemyShot)
            {
                PlayerController player = hitTransform.GetComponentInParent<PlayerController>();
                if (player)
                {
                    isEnemyShot = true;
                    gameObject.SetActive(false);
                    player.PlayerDie(direction,hitTransform.position);
                }
            }
        }

        private void Rotate()
        {
            visualTransform.Rotate(Vector3.forward, 1200 * Time.deltaTime, Space.Self);
        }

        public float GetBulletSpeed()
        {
            return shootingForce;
        }

        internal Transform GetHitEnemyTransform()
        {
            return hitTransform;
        }
    }
}