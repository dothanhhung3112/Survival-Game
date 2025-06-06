﻿using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Hung.Tools
{
    [RequireComponent(typeof(TimeScaleController))]
	public class BulletTimeController : MonoBehaviour
	{
		[Serializable]
		public class TargetTrackingSetup
		{
			public CinemachinePathController avaliableTrack;
			public CameraCartController avaliableDolly;
		}

		[Serializable]
		public class BulletTrackingSetup : TargetTrackingSetup
		{
			public float minDistance;
			public float maxDistance;
		}

		[SerializeField] private CinemachineBrain cameraBrain;
		[SerializeField] private BulletTrackingSetup[] bulletTackingSetup;
		[SerializeField] private TargetTrackingSetup[] enemyTrackingSetup;
		[SerializeField] private PlayerShootingController shootingController;
		[SerializeField] private float distanceToChangeCamera;
		[SerializeField] private float finishingCameraDuration;

		private TimeScaleController timeScaleController;
		public CinemachineSmoothPath trackInstance;
		private CameraCartController dollyInstance;
		private Bullet activeBullet;
		private Transform targetTransform;
		public List<TargetTrackingSetup> clearTracks = new List<TargetTrackingSetup>();
		private bool isLastCameraActive = false;

		private void Awake()
		{
			timeScaleController = GetComponent<TimeScaleController>();
		}

		internal void StartSequence(Bullet activeBullet, Transform targetTransform)
		{
			ResetVariables();
			float distanceToTarget = Vector3.Distance(activeBullet.transform.position, targetTransform.position);
			var setupsInRange = bulletTackingSetup.Where(s => distanceToTarget > s.minDistance && distanceToTarget < s.maxDistance).ToArray();
			//var selectedTrackingSetup = SelectTrackingSetup(activeBullet.transform, setupsInRange, activeBullet.transform.rotation);
			//if (selectedTrackingSetup == null)
			//	return;

			var selectedTrackingSetup = bulletTackingSetup[0];

			this.activeBullet = activeBullet;
			this.targetTransform = targetTransform;

			CreateBulletPath(activeBullet.transform, selectedTrackingSetup.avaliableTrack);
			CreateDolly(selectedTrackingSetup);
			cameraBrain.gameObject.SetActive(true);
			float speed = CalculateDollySpeed();
			dollyInstance.InitDolly(trackInstance, activeBullet.transform, speed);
		}

		private void CreateDolly(TargetTrackingSetup setup)
		{
			var selectedDolly = setup.avaliableDolly;
			dollyInstance = Instantiate(selectedDolly);
		}

		private void CreateBulletPath(Transform bulletTransform, CinemachinePathController selectedPath)
		{
			trackInstance = Instantiate(selectedPath.path, bulletTransform);
			trackInstance.transform.localPosition = selectedPath.transform.position;
			trackInstance.transform.localRotation = selectedPath.transform.rotation;
		}

		private float CalculateDollySpeed()
		{
			if (trackInstance == null || activeBullet == null)
				return 0f;

			float distanceToTarget = Vector3.Distance(activeBullet.transform.position, targetTransform.position);
			float speed = activeBullet.GetBulletSpeed();
			float pathDistance = trackInstance.PathLength;
			return pathDistance * speed / distanceToTarget;
		}


		private void CreateEnemyPath(Transform enemytransform, Transform bulletTransform, CinemachinePathController selectedPath)
		{
			Quaternion rotation = Quaternion.Euler(Vector3.up * bulletTransform.root.eulerAngles.y);
			trackInstance = Instantiate(selectedPath.path, enemytransform.position, rotation);
		}

		private TargetTrackingSetup SelectTrackingSetup(Transform trans, TargetTrackingSetup[] setups, Quaternion orientation)
		{
			clearTracks.Clear();
			for (int i = 0; i < setups.Length; i++)
			{
				if (CheckIfPathIsClear(setups[i].avaliableTrack, trans, orientation))
					clearTracks.Add(setups[i]);
			}
			if (clearTracks.Count == 0)
				return null;
			return clearTracks[UnityEngine.Random.Range(0, clearTracks.Count)];
		}

		private bool CheckIfPathIsClear(CinemachinePathController path, Transform trans, Quaternion orientation)
		{
			return path.CheckIfPathISClear(trans, Vector3.Distance(trans.position, targetTransform.position), orientation);
		}

		private void Update()
		{
			if (!activeBullet)
				return;

			if (CheckIfBulletIsNearTarget())
				ChangeCamera();
		}

		private bool CheckIfBulletIsNearTarget()
		{
			return Vector3.Distance(activeBullet.transform.position, targetTransform.position) < distanceToChangeCamera;
		}

		private void ChangeCamera()
		{
			if (isLastCameraActive)
				return;
			isLastCameraActive = true;
			DestroyCinemachineSetup();
			Transform hitTransform = activeBullet.GetHitEnemyTransform();
			if (hitTransform)
			{
				Quaternion rotation = Quaternion.Euler(Vector3.up * activeBullet.transform.rotation.eulerAngles.y);
				//var selectedTrackingSetup = SelectTrackingSetup(hitTransform, enemyTrackingSetup, rotation);
				//if (selectedTrackingSetup != null)
				var selectedTrackingSetup = enemyTrackingSetup[0];
				CreateEnemyPath(hitTransform, activeBullet.transform, selectedTrackingSetup.avaliableTrack);
				CreateDolly(selectedTrackingSetup);
				dollyInstance.InitDolly(trackInstance, hitTransform.transform);
				timeScaleController.SlowDownTime();
			}
			StartCoroutine(FinishSequence());
		}

		private void DestroyCinemachineSetup()
		{
			Destroy(trackInstance.gameObject);
			Destroy(dollyInstance.gameObject);
		}

		private IEnumerator FinishSequence()
		{
			yield return new WaitForSecondsRealtime(finishingCameraDuration);
			cameraBrain.gameObject.SetActive(false);
			timeScaleController.SpeedUpTime();
			DestroyCinemachineSetup();
			Destroy(activeBullet.gameObject);
			ResetVariables();
		}

		private void ResetVariables()
		{
			isLastCameraActive = false;
			trackInstance = null;
			dollyInstance = null;
			activeBullet = null;
			clearTracks.Clear();
			targetTransform = null;
		}
	}
}