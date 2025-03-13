using DG.Tweening;
using Hung.Gameplay.GameFight;
using UnityEngine;

public class DetectorNearest : MonoBehaviour
{
    [SerializeField] Transform target;

    void Update()
    {
        transform.LookAt(NearestTarget());
    }

    Transform NearestTarget()
    {
        Transform target = transform;
        float minDis = 100f;
        foreach (var item in GameFightController.Instance.enemys)
        {
            float distance = Vector3.Distance(transform.position, item.transform.position);
            if (distance < minDis)
            {
                minDis = distance;
                target = item.transform;
            }
        }
        return target;
    }
}
