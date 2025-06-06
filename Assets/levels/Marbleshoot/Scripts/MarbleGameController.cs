using _Scripts.Extension;
using DG.Tweening;
using Hung.UI;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Hung.Gameplay.Marble
{
    public class MarbleGameController : MonoBehaviour
    {
        public static MarbleGameController Instance;
        public bool isPlayerTurn = false, isWin = false, isLose = false;
        public int enemyMarble, playerMarble;
        public int enemyMarbleHoled, playerMarbleHoled;
        [SerializeField] Animator playerAnimator;
        [SerializeField] ParticleSystem effectWin;
        [SerializeField] Marble marblePrefab;
        [SerializeField] Transform marblePos;
        [SerializeField] MarbleEnemy enemy;
        [SerializeField] Animator pinkCharAnim;
        [SerializeField] GameObject camStart;
        [SerializeField] GameObject camWin;
        [SerializeField] GameObject camLose;
        Marble curMarble;
        bool isDragging = false;
        Vector2 startTouchPos;
        Vector2 endTouchPos;
        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        private void Start()
        {
            SwitchPlayerTurn();
            isPlayerTurn = false;
            SoundManager.Instance.PlayBGMusic6();
            UIMarbleController.Instance.UIGamePlay.UpdateTextEnemyMarble(0);
            UIMarbleController.Instance.UIGamePlay.UpdateTextPlayerMarble(0);
        }

        private void Update()
        {
            if (isWin || isLose) return;
            if (Input.GetMouseButtonDown(0) && isPlayerTurn && !isWin && !isLose && !UIDetection.IsPointerOverUIObject())
            {
                isDragging = true;
                startTouchPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0) && isPlayerTurn && isDragging)
            {
                isDragging = false;
                isPlayerTurn = false;
                playerMarble--;
                UIMarbleController.Instance.UIGamePlay.UpdatePlayerMarbleUI(playerMarble);
                endTouchPos = Input.mousePosition;
                Vector2 dragVector = endTouchPos - startTouchPos;
                float forceAmount = dragVector.magnitude * 0.001f;
                float angleY = Mathf.Atan2(dragVector.x, dragVector.y) * Mathf.Rad2Deg;
                Vector3 throwDirection = Quaternion.Euler(-30, Mathf.Clamp(angleY, -30, 30), 0) * Vector3.forward;
                curMarble.Shoot(throwDirection * forceAmount);
                SoundManager.Instance.PlaySoundMarbleShoot();
                StartCoroutine(SwitchEnemyTurn());
            }
        }

        public void StartGame()
        {
            camStart.SetActive(false);
            DOVirtual.DelayedCall(1f, delegate
            {
                isPlayerTurn = true;
            });
        }

        public void CheckEndGame()
        {
            if (isWin || isLose) return;
            if (enemyMarble == 0 && playerMarble == 0)
            {
                if (playerMarbleHoled > enemyMarbleHoled)
                {
                    StartCoroutine(Win());
                }
                else
                {
                    StartCoroutine(Lose());
                }
            }
        }

        public void Revive()
        {
            isLose = false;
            playerAnimator.Play("RunFight");
            playerMarble += 2;
            UIMarbleController.Instance.UIGamePlay.UpdatePlayerMarbleUI(playerMarble);
            camLose.SetActive(false);
            DOVirtual.DelayedCall(1f, delegate
            {
                SwitchPlayerTurn();
            });
        }

        public Marble SpawnMarble(Vector3 position)
        {
            Marble marble;
            marble = Instantiate(marblePrefab, position, Quaternion.identity);
            return marble;
        }

        public void IncreaseMarbleHoled(bool isEnemy)
        {
            if (isEnemy)
            {
                enemyMarbleHoled++;
                UIMarbleController.Instance.UIGamePlay.UpdateTextEnemyMarble(enemyMarbleHoled);
            }
            else
            {
                playerMarbleHoled++;
                UIMarbleController.Instance.UIGamePlay.UpdateTextPlayerMarble(playerMarbleHoled);
            }
        }

        public void SwitchPlayerTurn()
        {
            if (isWin || isLose) return;
            isPlayerTurn = true;
            curMarble = SpawnMarble(marblePos.position);
        }

        IEnumerator SwitchEnemyTurn()
        {
            yield return new WaitForSeconds(3f);
            if(enemyMarble <= 0)
            {
                SwitchPlayerTurn();
                yield break;
            }
            enemyMarble--;
            UIMarbleController.Instance.UIGamePlay.UpdateEnemyMarbleUI(enemyMarble);
            enemy.ThrowMarble();
        }

        IEnumerator Win()
        {
            isWin = true;
            UIMarbleController.Instance.UIGamePlay.DisplayPanelGameplay(false);
            SoundManager.Instance.StopMusic();
            SoundManager.Instance.PlaySoundWin();
            pinkCharAnim.transform.DOLookAt(enemy.transform.position, 1f);
            yield return new WaitForSeconds(1f);
            camWin.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            pinkCharAnim.Play("Shooting");
            DOVirtual.DelayedCall(0.1f, delegate
            {
                enemy.Die();
            });
            effectWin.Play();
            playerAnimator.Play("Win");
            yield return new WaitForSeconds(5f);
            UIMarbleController.Instance.UIWin.DisplayPanelWin(true);
        }

        IEnumerator Lose()
        {
            isLose = true;
            UIMarbleController.Instance.UIGamePlay.DisplayPanelGameplay(false);
            SoundManager.Instance.StopMusic();
            SoundManager.Instance.PlaySoundLose();
            if (!Manager.Instance.isRevived)
            {
                UIRevive.Instance.DisplayRevivePanel(true);
                yield break;
            }
            yield return new WaitForSeconds(2f);
            camLose.SetActive(true);
            yield return new WaitForSeconds(2f);
            playerAnimator.Play("die1");
            ObjectPooler.instance.SetObject("bloodEffect", transform.position + new Vector3(0, 0.5f, 0));
            yield return new WaitForSeconds(5f);
            UIMarbleController.Instance.UILose.DisplayPanelLose(true);
        }
    }
}
