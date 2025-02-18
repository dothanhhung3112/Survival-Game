using UnityEngine;

namespace Hung.Tools
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimationController : MonoBehaviour
    {
        private Animator animator;
        [SerializeField] private ParticleSystem shootEffect;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void DisableAnimator()
        {
            animator.enabled = false;
        }

        public void PlaySoundShoot()
        {
            if(SoundManager.Instance!= null) 
            SoundManager.Instance.PlaySoundGunShooting();

            if(shootEffect!=null)
            shootEffect.Play();
        }
    }
}