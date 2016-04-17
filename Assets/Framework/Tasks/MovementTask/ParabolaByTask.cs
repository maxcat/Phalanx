using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParabolaByTask : Task {

	#region Fields
	protected float linearSpeed;
	protected float accelerationY;
	protected float speedY;

	protected float baseAccelerationY;
	#endregion

	#region Constructor
	public ParabolaByTask(MonoBehaviour mono, GameObject owner, float speed, float accelerationY, Vector3 offset, Vector3 fixSpeedDirection,  bool rotateObject = false)
		: base (mono)
	{
		coroutine = moveFlow(owner, speed, accelerationY, offset, fixSpeedDirection, rotateObject);
	}
	#endregion

	#region Protected Functions
	protected IEnumerator moveFlow(GameObject owner, float speed, float accelerationY, Vector3 offset, Vector3 fixSpeedDirection, bool rotateObject)
	{
		Vector3 startPos = owner.transform.position;
		Vector3 targetPos = startPos + offset;
		Vector3 offsetXZ = offset - Vector3.up * offset.y;


		float duration;

		if(fixSpeedDirection == Vector3.right)
		{
			duration = Mathf.Abs(offset.x) / Mathf.Abs(speed);
		}
		else if(fixSpeedDirection == Vector3.forward)
		{
			duration = Mathf.Abs(offset.z) / Mathf.Abs(speed);
		}
		else
		{
			duration = offsetXZ.magnitude / Mathf.Abs(speed);
		}

		Vector3 linearDirection = offsetXZ;
		linearDirection.Normalize();

		float linearDistance = offsetXZ.magnitude;

		Vector3 vLinearSpeed = offsetXZ / duration;

		linearSpeed = vLinearSpeed.magnitude;

		baseAccelerationY = accelerationY;

		accelerationY = baseAccelerationY * Mathf.Pow(taskSpeed, 2f);

		speedY = (offset.y + accelerationY * Mathf.Pow(duration / taskSpeed, 2f) / 2) / (duration / taskSpeed);

		float linearTravelDistance = 0f;

		float previousAcceleration = accelerationY;
		while(linearTravelDistance < linearDistance)
		{
			float deltaTime = Time.deltaTime;

			linearTravelDistance += linearSpeed * deltaTime * taskSpeed;
			Vector3 linearOffset =  linearDirection * linearSpeed * deltaTime * taskSpeed;

			previousAcceleration = accelerationY;
			accelerationY = baseAccelerationY * Mathf.Pow(taskSpeed, 2f);

			float deltaDisY = speedY * deltaTime - 
				Mathf.Pow(deltaTime, 2f) * accelerationY / 2;

			if(rotateObject)
				owner.transform.rotation = Quaternion.LookRotation(
					new Vector3(
						vLinearSpeed.x * taskSpeed,
						speedY,
						vLinearSpeed.z * taskSpeed));

			owner.transform.position += linearOffset + Vector3.up * deltaDisY;

			if(accelerationY == previousAcceleration)
				speedY = speedY - accelerationY * deltaTime;
			else
				speedY = speedY * taskSpeed - accelerationY * deltaTime;

			yield return new WaitForFixedUpdate();
		}


		owner.transform.position = targetPos;
		yield return new WaitForFixedUpdate();
	}
		
	#endregion

}
