using UnityEngine;

namespace Hung.Gameplay.TugOfWar
{
    public class TugOfWarCharacter : MonoBehaviour
    {
        public bool active;
        public Animator anim;
        float blend = 0f;
        int blendHash;
        public bool canFall = false;
        // Start is called before the first frame update
        void Start()
        {
            active = true;
            anim = GetComponent<Animator>();
            blendHash = Animator.StringToHash("Blend");
        }

        private void Update()
        {
            if (canFall)
            {
                blend += Time.deltaTime * 2f;
                if (blend > 1) blend = 1;
                anim.SetFloat(blendHash, blend);
            }
        }
    }
}
