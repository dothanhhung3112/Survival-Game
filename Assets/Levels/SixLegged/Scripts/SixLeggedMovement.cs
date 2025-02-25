using DG.Tweening;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Animator[] animators;
    DOTweenPath path;
    [SerializeField] float accleration;
    float speed;
    int speedToHash;
    bool canMove = false;

    private void Start()
    {
        path = GetComponent<DOTweenPath>();
        speedToHash = Animator.StringToHash("Speed");
        path.DOPlay();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            canMove = !canMove;
        }

        if (canMove)
        {
            path.DOPlay();
            speed += Time.deltaTime * accleration;
            if(speed > 1) speed = 1;
            foreach(var item in animators)
            {
                item.SetFloat(speedToHash,speed);
            }
        }
        else
        {
            path.DOPause();
            speed -= Time.deltaTime * accleration;
            if (speed <= 0) speed = 0;
            foreach (var item in animators)
            {
                item.SetFloat(speedToHash, speed);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Point"))
        {

        }
    }
}
