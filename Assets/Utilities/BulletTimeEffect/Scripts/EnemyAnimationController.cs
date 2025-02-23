using UnityEngine;

namespace Hung.Tools
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimationController : MonoBehaviour
    {
        private Animator animator;
        [SerializeField] private ParticleSystem shootEffect;
        [SerializeField] private bool noAnim;

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
            if (noAnim) return;
            if(SoundManager.Instance!= null) 
            SoundManager.Instance.PlaySoundGunShooting();

            if(shootEffect!=null)
            shootEffect.Play();
        }
    }
}