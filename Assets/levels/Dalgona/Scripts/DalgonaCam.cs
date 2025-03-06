using UnityEngine;
using DG.Tweening;

namespace Hung.Gameplay.Dalgona
{
    public class DalgonaCam : MonoBehaviour
    {
        Sequence sequence;
        public Ease ease;
        public GameObject camLose, camWin,camTable;
        public Animator anim_player, anim_enemy;
        float blend = 0;
        int blendHash;
        bool canBlend = false;
        bool hasAnimated = false;

        private void Start()
        {
            blendHash = Animator.StringToHash("Blend");

        }

        private void Update()
        {
            //if (canBlend)
            //{
            //    blend += Time.deltaTime * 2f;
            //    if (blend > 1)
            //    {
            //        blend = 1;
            //        if (!hasAnimated)
            //        {
            //            hasAnimated = true;
            //            anim_enemy.Play("Shooting");
            //        }
            //    }
            //    anim_enemy.SetFloat(blendHash, blend);
            //}
        }

        public void win_move()
        {
            camWin.SetActive(true);
            DOVirtual.DelayedCall(2f, delegate
            {
                DalgonaController.Instance.boxDalgona.camBox.SetActive(false);
                animate_player();
            });
            anim_enemy.gameObject.SetActive(false);
        }

        public void lose_move()
        {
            camLose.SetActive(true);
            DOVirtual.DelayedCall(2f, delegate
            {
                animate_enemy();
            });
        }

        public void animate_enemy()
        {
            //SoundManager.instance.Play("fire2");
            anim_enemy.Play("Shooting");
            SoundManager.Instance.PlaySoundGunShooting();
            DOVirtual.DelayedCall(0.2f, delegate
            {
                anim_player.Play("Die");
                GameObject blood = ObjectPooler.instance.SetObject("bloodEffect", anim_player.transform.position + new Vector3(0, 55, 0));
                anim_player.transform.localPosition += new Vector3(0, 8, 0);
                blood.transform.localScale *= 50f;
                SoundManager.Instance.PlaySoundMaleHited();
            });
        }

        public void animate_player()
        {
            anim_player.Play("happy");
        }
    }
}
