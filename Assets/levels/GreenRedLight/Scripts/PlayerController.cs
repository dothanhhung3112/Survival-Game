using System.Collections;
using UnityEngine;
using CnControls;
using System.Collections.Generic;
using System.Linq;
using Hung.UI;
using DG.Tweening;

namespace Hung.Gameplay.GreenRedLight {

	[RequireComponent(typeof(Rigidbody))]
	public class PlayerController : MonoBehaviour
	{
		[Header(" Control Settings ")]
		public Rigidbody thisRigidbody;
		public SimpleJoystick joystick;
		public float moveSpeed;
		public static bool canMove;
		public bool GmRun, die, win;

		[Header("Animation")]
		[SerializeField] CutScenelevel1 cutScene;
		Animator animator;
        public enemCtr ec;
        float velocity = 0f;
		public float acceleration = 0.2f;
		int veclocityHash;
		int blendHash;
		bool canRandom = false;
		float[] runPoses = { 0,0.5f,1};
		int randomePoseIndex;
        Vector3 scaledMovement;
        [SerializeField] List<Rigidbody> listRB;
		[SerializeField] List<CharacterJoint> listCJ;
		[SerializeField] List<Collider> listCD;
		[SerializeField] List<ActiveJoin> listActiveJoin;


		// Use this for initialization
		void Start()
		{
			// Store some values
			canMove = true;

			animator = GetComponent<Animator>();
			veclocityHash = Animator.StringToHash("velocity");
			blendHash = Animator.StringToHash("Blend");
            ec = FindAnyObjectByType<enemCtr>();
            listRB.AddRange(gameObject.GetComponentsInChildren<Rigidbody>().Where(x => x.gameObject != this.gameObject));
			listCD.AddRange(gameObject.GetComponentsInChildren<Collider>().Where(x => x.gameObject != this.gameObject));
			listCJ.AddRange(gameObject.GetComponentsInChildren<CharacterJoint>());
			DisActiveRagdoll();
		}

		void Update()
		{
            if (GmRun && !die && !win)
			{
				if (joystick.HorizintalAxis.Value != 0 || joystick.VerticalAxis.Value != 0)
				{
					if (ec.animcor)
					{
						GetComponent<HighlightPlus.HighlightEffect>().highlighted = true;
						die = true;
						cutScene.CutSceneLose();
					}
					 
                    animator.Play("runPose");
					SetRandomPoseWhileRunning(false);
					Move();
                    animator.speed = 1;
				}
				else
				{
					animator.speed = 0;
					SetRandomPoseWhileRunning(true);
				}
			}

			if (die)
			{
                SetRandomPoseWhileRunning(true);
            }

			if (UIGreenRedLightController.Instance.time <= 0 && !win && !die)
			{
				die = true;
				cutScene.CutSceneLose();
			}
		}

		void SetRandomPoseWhileRunning(bool enable)
		{
			if (enable)
			{
				canRandom = true;
				velocity += Time.deltaTime * acceleration;
				if (velocity > 1)
				{
                    velocity = 1;
				}
				animator.SetFloat(veclocityHash, velocity);
			}
			else
			{
				velocity -= Time.deltaTime * acceleration * 2;
				if (velocity <= 0)
				{
					velocity = 0;
                    if (canRandom)
                    {
                        randomePoseIndex = Random.Range(0, runPoses.Length);
                        canRandom = false;
						animator.SetFloat(blendHash, runPoses[randomePoseIndex]);
                    }
                }
                animator.SetFloat(veclocityHash, velocity);
			}
		}

		float elapsedTime = 0;
		public void Move()
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > 0.45f)
			{
				SoundManager.Instance.PlaySoundWalk();
				elapsedTime = 0;
			}
            scaledMovement = new Vector3(joystick.HorizintalAxis.Value, 0, joystick.VerticalAxis.Value) * moveSpeed * Time.deltaTime;
            transform.LookAt(transform.position + scaledMovement);
            transform.Translate(scaledMovement, Space.World);
        }

		public void PlayerDie(Vector3 direction,Vector3 hitPoint)
		{
			UIGreenRedLightController.Instance.canCountTime = false;
			SoundManager.Instance.PlaySoundMaleHited();
			GameObject gm = ObjectPooler.instance.SetObject("bloodEffect", hitPoint);
			animator.Play("die1");
            //animator.enabled = false;
            //ActiveRagdoll();
            //listRB[9].AddForce(direction * 4f, ForceMode.Impulse);
            StartCoroutine(dieplayer());
		}

        public void Revive()
        {
            die = false;
			ec.StartSingAgain();
            joystick.HorizintalAxis.Value = 0;
			joystick.VerticalAxis.Value = 0;
			GetComponent<HighlightPlus.HighlightEffect>().highlighted = false;
            animator.Play("runPose");
        }

		public void PowerUp(bool isIngame)
		{
			moveSpeed += isIngame ? moveSpeed : moveSpeed * 2;
		}

        IEnumerator dieplayer()
		{
            SoundManager.Instance.PlaySoundLose();
			yield return new WaitForSeconds(5f);
			UIGreenRedLightController.Instance.UILose.DisplayPanelLose(true);
        }

		private void OnCollisionEnter(Collision collision)
		{
            if (win || die) return;
            if (collision.gameObject.tag == "win")
			{
                win = true;
				transform.DOMoveZ(transform.position.z + 1f, 1f);
                animator.SetFloat(veclocityHash, 0);
                DOVirtual.DelayedCall(1f, delegate
				{
					StartCoroutine(WinPlayer());
				});
			}
		}

		IEnumerator WinPlayer()
		{
            UIGreenRedLightController.Instance.canCountTime = false;
			transform.eulerAngles = new Vector3(0, 180, 0);
			GetComponent<Animator>().Play("Win");
			GetComponent<Animator>().speed = 1;
			SoundManager.Instance.PlaySoundWin();
			yield return new WaitForSeconds(7f);
            UIGreenRedLightController.Instance.UIGamePlay.DisplayPanelGameplay(false);
            UIGreenRedLightController.Instance.UIWin.DisplayPanelWin(true);
		}

		void DisActiveRagdoll()
		{
			foreach (var item in listCJ)
			{
                if (!item.TryGetComponent(out ActiveJoin activeJoin))
                {
                    activeJoin = item.gameObject.AddComponent<ActiveJoin>();
                }
                listActiveJoin.Add(activeJoin);
            }
			foreach (var item in listRB)
			{
				item.isKinematic = true;
			}
			foreach (var item in listCD)
			{
				item.enabled = false;
			}
			for (int i = 0; i < listActiveJoin.Count; i++)
			{
				listActiveJoin[i].CopyValuesAndDestroyJoint(listCJ[i]);
			}
		}

		void ActiveRagdoll()
		{
            foreach (var item in listRB)
			{
				item.isKinematic = false;
			}
			foreach (var item in listCD)
			{
				item.enabled = true;
			}
			foreach (var item in listActiveJoin)
			{
				item.CreateJointAndDestroyThis();
			}
		}
	}
}