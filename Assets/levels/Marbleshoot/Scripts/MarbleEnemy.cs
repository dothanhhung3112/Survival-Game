using DG.Tweening;
using UnityEngine;

namespace Hung.Gameplay.Marble
{
    public class MarbleEnemy : MonoBehaviour
    {
        [SerializeField] private Transform marblePos;
        [SerializeField] private Marble marblePrefab;
        [SerializeField] private Transform marbleParent;
        Animator animator;
        Marble marble;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void ThrowMarble()
        {
            marble = Instantiate(marblePrefab, marbleParent);
            marble.transform.position = marblePos.transform.position;
            marble.transform.localScale = marble.transform.localScale/0.7f;
            animator.Play("Throw");
            DOVirtual.DelayedCall(0.6f, delegate
            {
                marble.gameObject.SetActive(false);
                Marble curMarble = Instantiate(marblePrefab, marblePos.transform.position, Quaternion.identity);
                Vector3 throwDirection = Quaternion.Euler(Random.Range(-155, -145), Random.Range(-30, 30), 0) * Vector3.forward;
                curMarble.Shoot(throwDirection * 0.4f);

                DOVirtual.DelayedCall(3f, delegate
                {
                    MarbleGameController.Instance.SwitchPlayerTurn();
                });
            });
        }

        public void Die()
        {
            SoundManager.Instance.PlaySoundGunShooting();
            SoundManager.Instance.PlaySoundMaleHited();
            animator.Play("Die");
            ObjectPooler.instance.SetObject("bloodEffect", transform.position + new Vector3(0, 0.5f, 0));
        }
    }
}