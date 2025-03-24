using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Hung.Tools;
using Hung.UI;
using ACEPlay.Bridge;

namespace Hung.Gameplay.GreenRedLight
{
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
        [SerializeField] CinemachineVirtualCamera enemyCam;
        [SerializeField] ParticleSystem vfxShooting;

        [SerializeField] GameObject playerArrow;

        public void CutSceneLose()
        {
            playerArrow.SetActive(false);
            UIGreenRedLightController.Instance.UIGamePlay.DisplayPanelGameplay(false);
            enemyAnimator.Play("Shooting");
            DOVirtual.DelayedCall(0.8f, delegate
            {
                MoveCamToEnemy();
                enemyAnimator.transform.LookAt(hitTransform.position);
            });

            DOVirtual.DelayedCall(1f, delegate
            {
                enemyAnimator.speed = 0.5f;
                DOVirtual.DelayedCall(0.3f, delegate
                {
                    SoundManager.Instance.PlaySoundGunShooting();
                    vfxShooting.Play();
                    Shoot();
                });
            });
        }

        public void MoveCamToEnemy()
        {
            enemyCam.gameObject.SetActive(true);
        }

        public void Shoot()
        {
            Vector3 direction = hitTransform.transform.position - bulletSpawnTransform.position;
            Bullet bulletInstance = Instantiate(bulletPrefab, bulletSpawnTransform.position, Quaternion.LookRotation(direction));
            bulletInstance.Launch(shootingForce, hitTransform, hitTransform.position);
            bulletTimeController.StartSequence(bulletInstance, hitTransform);
        }
    }
}
