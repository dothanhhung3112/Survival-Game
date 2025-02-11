using System.Collections;
using UnityEngine;
using CnControls;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

	[Header(" Control Settings ")]
	public Rigidbody thisRigidbody;
	public SimpleJoystick joystick;
	public float moveSpeed;
	public float RotSpeed;
	public float maxX;
	public float maxZ;
	bool move;
	public static bool canMove;
	public bool GmRun,die,chwya,win;

	[Header("Animation")]
	Animator animator;
	float velocity = 0f;
	public float acceleration = 0.2f;
	int veclocityHash;
	
	// Use this for initialization
	void Start () {
		
		// Store some values
		Application.targetFrameRate = 60;
		canMove = true;

		animator = GetComponent<Animator>();
		veclocityHash = Animator.StringToHash("velocity");
	}
	
	void Update () {
		//joystick.
		if(GmRun && !die && !win)
        {
			if (joystick.HorizintalAxis.Value != 0 || joystick.VerticalAxis.Value != 0)
			{
				if(FindObjectOfType<enemCtr>().animcor)
                {
					GetComponent<HighlightPlus.HighlightEffect>().highlighted = true;
					die = true;
					StartCoroutine(dieplayer());
                }
				// Move Player
				move = true;
				animator.Play("Run");
				velocity = 0;
                animator.SetFloat(veclocityHash, velocity);
                transform.forward = new Vector3(joystick.HorizintalAxis.Value * Time.deltaTime, 0, joystick.VerticalAxis.Value * Time.deltaTime);
				GetComponent<Animator>().speed = 1;
			}
			else
			{
				move = false;
                GetComponent<Animator>().speed = 0;
				velocity += Time.deltaTime * acceleration;
				animator.SetFloat(veclocityHash, velocity);

            }
        }

		if(die && !chwya)
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

		if(UIGreenRedLightController.Instance.time <=0 && !win && !die)
		{
			die = true;
			StartCoroutine(dieplayer());
		}
	}

	private void FixedUpdate() {
		if(GmRun && !die && !win)
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

	IEnumerator dieplayer()
    {
		yield return new WaitForSeconds(0.5f);
		chwya = true;
		GetComponent<BoxCollider>().isTrigger = true;
		SoundManager.Instance.PlaySoundGunShooting();
		yield return new WaitForSeconds(0.1f);
        SoundManager.Instance.PlaySoundMaleHited();
		GameObject gm = ObjectPooler.instance.SetObject("bloodEffect",transform.position);
		gm.transform.localPosition = new Vector3(0, 1.3f, 0);
		int bb = Random.Range(1, 5);
		GetComponent<HighlightPlus.HighlightEffect>().highlighted = false;
		GetComponent<Animator>().Play("die" + bb.ToString());
		GetComponent<Animator>().speed = 1;
        SoundManager.Instance.PlaySoundLose();
        yield return new WaitForSeconds(5f);
		UIGreenRedLightController.Instance.UILose.DisplayPanelLose(true);
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="win")
        {
			win = true;
			StartCoroutine(winplayer());
        }
    }

	IEnumerator winplayer()
    {
		transform.eulerAngles = new Vector3(0, 180, 0);
		//FindObjectOfType<UiManager>().wineffet.SetActive(true);
		GetComponent<Animator>().Play("win");
		GetComponent<Animator>().speed = 1;
		SoundManager.Instance.PlaySoundWin();
		yield return new WaitForSeconds(7f);

        UIGreenRedLightController.Instance.UIWin.DisplayPanelWin(true);
    }
}