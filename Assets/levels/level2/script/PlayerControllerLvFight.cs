using CnControls;
using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControllerLvFight : MonoBehaviour
{
    [SerializeField] private SimpleJoystick joystick;
    Animator animator;
    NavMeshAgent navMeshAgent;

    private Vector3 scaledMovement;
    [SerializeField] GameObject lookAt;
    private int speedToHash;
    bool isHitting = false; 

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        speedToHash = Animator.StringToHash("Speed");
        lookAt = gameObject;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Hit");
        }
        scaledMovement = navMeshAgent.speed * Time.deltaTime * new Vector3(joystick.HorizintalAxis.Value,0,joystick.VerticalAxis.Value);
        navMeshAgent.Move(scaledMovement);
        transform.LookAt(lookAt.transform.position + scaledMovement);
        float joystickMagnitude = new Vector2(joystick.HorizintalAxis.Value,joystick.VerticalAxis.Value).magnitude;
        animator.SetFloat(speedToHash, Mathf.Clamp01(joystickMagnitude == 0 ? navMeshAgent.velocity.magnitude : joystickMagnitude));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Bot"))
        {
            Debug.Log("name" + other.name);
            RaycastHit hit;
            Vector3 from = transform.position;
            Vector3 to = other.transform.position;
            from.y += 0.5f;
            to.y = from.y;
            Physics.Linecast(from, to, out hit);
            if(hit.collider != null)
            {
                if (isHitting) return;
                isHitting = true;
                lookAt = other.gameObject;
                animator.SetTrigger("Hit");
                DOVirtual.DelayedCall(0.2f, delegate
                {
                    lookAt = gameObject;
                    other.GetComponent<EnemyControllerLvFight>().Die();
                });
            }
        }
    }
}
