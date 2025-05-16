using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Extension
{
    public class CheckSizeCanvas : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private CanvasScaler canvasScaler;

        void Awake()
        {
            if (canvasScaler == null)
            {
                canvasScaler = GetComponent<CanvasScaler>();
            }
        }

        private void Start()
        {
            float aspect = (float)Screen.width / (float)Screen.height;
            canvasScaler.matchWidthOrHeight = aspect < 0.55f ? 0 : 1;
        }
    }
}