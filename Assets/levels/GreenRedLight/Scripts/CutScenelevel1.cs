using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Hung.Tools;
using Hung.UI;
using System.Collections;

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
        [SerializeField] CinemachineVirtualCamera playerCam;
        [SerializeField] ParticleSystem vfxShooting;
        [SerializeField] GameObject playerArrow;

        private void Start()
        {
            UIRevive.Instance.SetOnCloseAction(delegate
            {
                StartCoroutine(EndingCard());
            });
        }

        public void CutSceneLose()
        {
            playerArrow.SetActive(false);
            UIGreenRedLightController.Instance.canCountTime = false;
            UIGreenRedLightController.Instance.UIGamePlay.DisplayPanelGameplay(false);
            enemyAnimator.Play("Shooting");
            if (CanShowRevivePanel()) return;
            StartCoroutine(EndingCard());
        }

        IEnumerator EndingCard()
        {
            yield return new WaitForSeconds(0.8f);
            enemyCam.gameObject.SetActive(true);
            enemyAnimator.transform.LookAt(hitTransform.position);
            yield return new WaitForSeconds(0.2f);
            enemyAnimator.speed = 0.5f;
            yield return new WaitForSeconds(0.3f);
            SoundManager.Instance.PlaySoundGunShooting();
            vfxShooting.Play();
            Shoot();
            enemyCam.gameObject.SetActive(false);
        }

        bool CanShowRevivePanel()
        {
            if (!Manager.Instance.isRevived)
            {
                DOVirtual.DelayedCall(0.6f, delegate
                {
                    UIRevive.Instance.DisplayRevivePanel(true);
                });
                return true;
            }
            return false;
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
