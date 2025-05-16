using UnityEngine;
using DG.Tweening;

namespace Hung.Gameplay.Dalgona
{
    public class DalgonaCam : MonoBehaviour
    {
        Sequence sequence;
        public Ease ease;
        float blend = 0;
        int blendHash;
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

        

        
    }
}
