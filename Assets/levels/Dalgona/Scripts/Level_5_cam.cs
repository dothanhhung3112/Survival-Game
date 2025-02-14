using UnityEngine;
using DG.Tweening;

namespace Hung.Gameplay.Dalgona
{
    public class Level_5_cam : MonoBehaviour
    {

        public Transform win_cam_pos, lose_cam_pos;
        Sequence sequence;
        public Ease ease;
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
            if (canBlend)
            {
                blend += Time.deltaTime * 2f;
                if (blend > 1)
                {
                    blend = 1;
                    if (!hasAnimated)
                    {
                        hasAnimated = true;
                        anim_enemy.Play("Shooting");
                    }
                }
                anim_enemy.SetFloat(blendHash, blend);
            }
        }

        public void win_move()
        {
            Quaternion qt = win_cam_pos.transform.rotation;

            Vector3 vt = qt.eulerAngles;

            sequence = DOTween.Sequence();

            sequence

                        .Append(transform.DOMove(win_cam_pos.position, 1f).SetEase(ease))
                        .Join(transform.DORotate(vt, .5f).SetEase(ease))
                        .OnComplete(() => animate_player());

            anim_enemy.gameObject.SetActive(false);
        }

        public void lose_move()
        {
            Quaternion qt = lose_cam_pos.transform.rotation;

            Vector3 vt = qt.eulerAngles;

            sequence = DOTween.Sequence();

            sequence

                        .Append(transform.DOMove(lose_cam_pos.position, 1f).SetEase(ease))
                        .Join(transform.DORotate(vt, .5f).SetEase(ease))
                        .OnComplete(() => animate_enemy());

            anim_player.gameObject.SetActive(false);
        }

        public void animate_enemy()
        {
            //SoundManager.instance.Play("fire2");
            canBlend = true;
        }

        public void animate_player()
        {
            anim_player.Play("happy");
        }
    }
}
