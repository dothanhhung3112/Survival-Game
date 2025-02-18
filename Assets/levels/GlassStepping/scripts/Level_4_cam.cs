
using UnityEngine;
using DG.Tweening;

public class Level_4_cam : MonoBehaviour
{
    public Transform target; // Target to follow
    public Vector3 targetLastPos , ofsset;
    public float speed_cam;
    Tweener tween;
    public Ease ease;
    public bool is_active;


    void Start()
    {
        
    }

    void Update()
    {
        if (!is_active) return;
        
        if (targetLastPos == target.position) return;
        
        Vector3 distance = target.position + ofsset;
        distance.x = 0;

        tween.ChangeEndValue(distance, true).Restart();
        targetLastPos = distance;
    }

    public void start_follow()
    {

        ofsset = transform.position - target.position;

        Vector3 distance = target.position + ofsset;
        distance.x = 0;

        tween = transform.DOMove(distance, speed_cam).SetEase(ease).SetAutoKill(false);
        targetLastPos = distance;

        is_active = true;
    }
}
