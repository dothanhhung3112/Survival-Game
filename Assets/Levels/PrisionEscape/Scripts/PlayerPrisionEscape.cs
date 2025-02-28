using CnControls;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerPrisionEscape : MonoBehaviour
{
    [SerializeField] SimpleJoystick joystick;
    [SerializeField] List<Transform> botPos;
    public GameObject lookAt;
    Animator animator;
    NavMeshAgent navMeshAgent;
    public Vector3 scaledMovement;
    int speedToHash;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        speedToHash = Animator.StringToHash("Speed");
        lookAt = gameObject;
    }

    private void Update()
    {
        scaledMovement = navMeshAgent.speed * Time.deltaTime * new Vector3(joystick.HorizintalAxis.Value, 0, joystick.VerticalAxis.Value);
        navMeshAgent.Move(scaledMovement);
        transform.LookAt(lookAt.transform.position + scaledMovement);
        animator.SetFloat(speedToHash, GetSpeed());
    }

    public float GetSpeed()
    {
        float joystickMagnitude = new Vector2(joystick.HorizintalAxis.Value, joystick.VerticalAxis.Value).magnitude;
        return Mathf.Clamp01(joystickMagnitude == 0 ? navMeshAgent.velocity.magnitude : joystickMagnitude);
    }

    public Vector3 GetLookAtPos()
    {
        return lookAt.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bot"))
        {
            other.tag = "Player";
            BotPrisionEscape bot = other.GetComponent<BotPrisionEscape>();
            bot.SetActive(this);
        }
    }
}
