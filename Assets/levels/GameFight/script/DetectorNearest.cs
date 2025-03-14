using Hung.Gameplay.GameFight;
using UnityEngine;

public class DetectorNearest : MonoBehaviour
{
    [SerializeField] Transform target;
    bool isTurnOff;

    void Update()
    {
        if (GameFightController.Instance.isWin || GameFightController.Instance.isLose)
        {
            if (isTurnOff)
            {
                gameObject.SetActive(false);
            }
        }

            if (GameFightController.Instance.isWin || GameFightController.Instance.isLose) return;
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
