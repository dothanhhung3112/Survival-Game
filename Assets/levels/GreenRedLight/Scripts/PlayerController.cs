using System.Collections;
using UnityEngine;
using CnControls;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Hung.UI;

namespace Hung.Gameplay.GreenRedLight {

	[RequireComponent(typeof(Rigidbody))]
	public class PlayerController : MonoBehaviour
	{

		[Header(" Control Settings ")]
		public Rigidbody thisRigidbody;
		public SimpleJoystick joystick;
		public float moveSpeed;
		public float RotSpeed;
		public float maxX;
		public float maxZ;
		bool move;
		public static bool canMove;
		public bool GmRun, die, chwya, win;

		[Header("Animation")]
		[SerializeField] CutScenelevel1 cutScene;
		Animator animator;
		float velocity = 0f;
		public float acceleration = 0.2f;
		int veclocityHash;
		bool canRandom = false;
		string[] runPoses = { "runfe", "rundynamicpose", "rundancepose" };
		int randomePoseIndex;

		[SerializeField] List<Rigidbody> listRB;
		[SerializeField] List<CharacterJoint> listCJ;
		[SerializeField] List<Collider> listCD;
		[SerializeField] List<ActiveJoin> listActiveJoin;


		// Use this for initialization
		void Start()
		{

			// Store some values
			Application.targetFrameRate = 60;
			canMove = true;

			animator = GetComponent<Animator>();
			veclocityHash = Animator.StringToHash("velocity");

			listRB.AddRange(gameObject.GetComponentsInChildren<Rigidbody>().Where(x => x.gameObject != this.gameObject));
			listCD.AddRange(gameObject.GetComponentsInChildren<Collider>().Where(x => x.gameObject != this.gameObject));
			listCJ.AddRange(gameObject.GetComponentsInChildren<CharacterJoint>());
			DisActiveRagdoll();
		}

		void Update()
		{
			//joystick.
			if (Input.GetKeyDown(KeyCode.A))
			{
				ActiveRagdoll();
			}

			if (GmRun && !die && !win)
			{
				if (joystick.HorizintalAxis.Value != 0 || joystick.VerticalAxis.Value != 0)
				{
					if (FindObjectOfType<enemCtr>().animcor)
					{
						GetComponent<HighlightPlus.HighlightEffect>().highlighted = true;
						die = true;
						cutScene.CutSceneLose();
					}
					// Move Player
					move = true;
					SetRandomPoseWhileRunning(false);
					transform.forward = new Vector3(joystick.HorizintalAxis.Value * Time.deltaTime, 0, joystick.VerticalAxis.Value * Time.deltaTime);
					GetComponent<Animator>().speed = 1;
				}
				else
				{
					move = false;
					GetComponent<Animator>().speed = 0;
					SetRandomPoseWhileRunning(true);
				}
			}

			if (die && !chwya)
			{
				if (joystick.HorizintalAxis.Value != 0 || joystick.VerticalAxis.Value != 0)
				{
					// Move Player
					move = true;

					animator.Play("Run");
					transform.forward = new Vector3(joystick.HorizintalAxis.Value * Time.deltaTime, 0, joystick.VerticalAxis.Value * Time.deltaTime);
					GetComponent<Animator>().speed = 1;
				}
				else
				{
					move = false;
					GetComponent<Animator>().speed = 0;
				}
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
				if (velocity > 1) velocity = 1;
				animator.SetFloat(veclocityHash, velocity);
			}
			else
			{
				if (canRandom)
				{
					randomePoseIndex = Random.Range(0, runPoses.Length);
					canRandom = false;
				}
				velocity -= Time.deltaTime * acceleration * 2;
				if (velocity <= 0)
				{
					animator.Play(runPoses[randomePoseIndex]);
					velocity = 0;
				}
				animator.SetFloat(veclocityHash, velocity);
			}
		}

		private void FixedUpdate()
		{
			if (GmRun && !die && !win)
			{
				Vector3 pos = transform.position;
				pos.x = Mathf.Clamp(pos.x, -maxX, maxX);
				pos.z = Mathf.Clamp(pos.z, -maxZ, maxZ);
				transform.position = pos;

				if (canMove)
				{
					if (move)
						Move();
					else
						thisRigidbody.velocity = Vector3.zero;
				}
			}
			if (die && !chwya)
			{
				Vector3 pos = transform.position;
				pos.x = Mathf.Clamp(pos.x, -maxX, maxX);
				pos.z = Mathf.Clamp(pos.z, -maxZ, maxZ);
				transform.position = pos;

				if (canMove)
				{
					if (move)
						Move();
					else
						thisRigidbody.velocity = Vector3.zero;
				}
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

			Vector3 movement = new Vector3(joystick.HorizintalAxis.Value, 0, joystick.VerticalAxis.Value);
			movement *= moveSpeed * Time.deltaTime;

			thisRigidbody.velocity = movement;
		}

		public void PlayerDie(Vector3 direction)
		{
			chwya = true;
			UIGreenRedLightController.Instance.canCountTime = false;
			GetComponent<BoxCollider>().isTrigger = true;
			SoundManager.Instance.PlaySoundMaleHited();
			GameObject gm = ObjectPooler.instance.SetObject("bloodEffect", listRB[9].position);
			//int bb = Random.Range(1, 5);
			//GetComponent<Animator>().Play("die" + bb.ToString());
			//GetComponent<Animator>().speed = 1;
			animator.enabled = false;
			ActiveRagdoll();
			listRB[9].AddForce(direction * 2f, ForceMode.Impulse);
			StartCoroutine(dieplayer());
		}

		IEnumerator dieplayer()
		{
			SoundManager.Instance.PlaySoundLose();
			yield return new WaitForSeconds(3f);
			UIGreenRedLightController.Instance.UILose.DisplayPanelLose(true);
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.tag == "win")
			{
				win = true;
				StartCoroutine(WinPlayer());
			}
		}

		IEnumerator WinPlayer()
		{
			UIGreenRedLightController.Instance.canCountTime = false;
			transform.eulerAngles = new Vector3(0, 180, 0);
			//FindObjectOfType<UiManager>().wineffet.SetActive(true);
			GetComponent<Animator>().Play("win");
			GetComponent<Animator>().speed = 1;
			SoundManager.Instance.PlaySoundWin();
			yield return new WaitForSeconds(7f);

			UIGreenRedLightController.Instance.UIWin.DisplayPanelWin(true);
		}

		void DisActiveRagdoll()
		{
			foreach (var item in listCJ)
			{
				listActiveJoin.Add(item.AddComponent<ActiveJoin>());
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