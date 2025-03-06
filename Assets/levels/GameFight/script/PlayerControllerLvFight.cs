using CnControls;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace Hung.Gameplay.GameFight
{
    public class PlayerControllerLvFight : MonoBehaviour
    {
        [SerializeField] SimpleJoystick joystick;
        [SerializeField] GameObject knife;
        GameObject lookAt;
        Animator animator;
        NavMeshAgent navMeshAgent;
        Vector3 scaledMovement;
        int speedToHash;
        float blendWining = 0;
        bool isHitting = false;
        bool isWinning = false;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            speedToHash = Animator.StringToHash("Speed");
            lookAt = gameObject;
        }

        private void Update()
        {
            if (!GameFightController.Instance.canCountTime || GameFightController.Instance.isWin || GameFightController.Instance.isLose) return;
            if (isWinning)
            {
                blendWining += Time.deltaTime * 3f;
                animator.SetFloat("Blend", blendWining);
            }
            scaledMovement = navMeshAgent.speed * Time.deltaTime * new Vector3(joystick.HorizintalAxis.Value, 0, joystick.VerticalAxis.Value);
            navMeshAgent.Move(scaledMovement);
            transform.LookAt(lookAt.transform.position + scaledMovement);
            float joystickMagnitude = new Vector2(joystick.HorizintalAxis.Value, joystick.VerticalAxis.Value).magnitude;
            animator.SetFloat(speedToHash, Mathf.Clamp01(joystickMagnitude == 0 ? navMeshAgent.velocity.magnitude : joystickMagnitude));
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Bot"))
            {
                RaycastHit hit;
                Vector3 from = transform.position;
                Vector3 to = other.transform.position;
                from.y += 0.5f;
                to.y = from.y;
                Physics.Linecast(from, to, out hit);
                if (hit.collider != null)
                {
                    if (isHitting) return;
                    EnemyControllerLvFight enemy = other.GetComponent<EnemyControllerLvFight>();
                    if (!GameFightController.Instance.IsEnemysContain(enemy)) return;
                    GameFightController.Instance.UpdateEnemy(enemy);
                    isHitting = true;
                    lookAt = other.gameObject;
                    animator.SetTrigger("Hit");
                    SoundManager.Instance.PlaySoundSlash();
                    navMeshAgent.speed /= 2;
                    DOVirtual.DelayedCall(0.35f, delegate
                    {
                        navMeshAgent.speed *= 2;
                        enemy.Die();
                    });
                    DOVirtual.DelayedCall(0.35f, delegate
                    {
                        lookAt = gameObject;
                        isHitting = false;
                    });
                }
            }
        }

        public void Die()
        {
            SoundManager.Instance.PlaySoundGunShooting();
            SoundManager.Instance.PlaySoundMaleHited();
            animator.Play("die1");
            ObjectPooler.instance.SetObject("bloodEffect", transform.position + new Vector3(0, 0.5f, 0));
        }

        public void Win()
        {
            animator.SetFloat(speedToHash, 0);
            Vector3 camPos = Camera.main.transform.position;
            DOVirtual.DelayedCall(1f, delegate
            {
                knife.SetActive(false);
                transform.DOLookAt(new Vector3(camPos.x, 0, camPos.z), 1f).OnComplete(delegate
                {
                    animator.Play("Dance3");
                    isWinning = true;
                });
            });
        }
    }
}
