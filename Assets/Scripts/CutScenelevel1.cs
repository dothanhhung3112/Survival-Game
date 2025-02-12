using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScenelevel1 : MonoBehaviour
{
    [Header("BulletEffectSlowTime")]
    [SerializeField] BulletTimeController bulletTimeController;
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform bulletSpawnTransform;
    [SerializeField] private float shootingForce;
    [SerializeField] private float minDistanceToPlayAnimation;
    [SerializeField] private Transform hitTransform;

    [Header("EnemyAnim")]
    [SerializeField] Animator enemyAnimator;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            enemyAnimator.transform.LookAt(hitTransform.position);
            enemyAnimator.Play("Shooting");
            Shoot();
        }
    }

    public void Shoot()
    {
        Vector3 direction = hitTransform.transform.position - bulletSpawnTransform.position;    
        Bullet bulletInstance = Instantiate(bulletPrefab, bulletSpawnTransform.position, Quaternion.LookRotation(direction));
        bulletInstance.Launch(shootingForce, hitTransform, hitTransform.position);
        bulletTimeController.StartSequence(bulletInstance, hitTransform.position);
    }
}
