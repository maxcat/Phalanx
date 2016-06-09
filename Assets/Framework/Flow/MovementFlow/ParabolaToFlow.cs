using UnityEngine;
using System.Collections;

public class ParabolaToFlow : Flow {

	#region Fields
	protected float 			linearSpeed;
	protected float 			accelerationY;
	protected float 			speedY;
	protected float 			baseAccelerationY;
	#endregion


	#region Constructor
	public ParabolaToFlow(GameObject owner, Vector3 targetPos, float offsetY, float speed, bool rotateObject = false)
		: base ()
	{
		source = moveWithHeight(owner, targetPos, offsetY, speed, rotateObject);
	}

	public ParabolaToFlow(GameObject owner, float accelerationY, float speed, Vector3 targetPos, Vector3 fixSpeedDirection, bool rotateObject = false)
		: base ()
	{
		source = moveWithAcceleration(owner, accelerationY, speed, targetPos, fixSpeedDirection, rotateObject);
	}
		
	#endregion

#region Protected Functions
	protected IEnumerator moveWithAcceleration(GameObject owner, float accelerationY, float speed, Vector3 targetPos, Vector3 fixSpeedDirection, bool rotateObject)
	{
		Vector3 startPos = owner.transform.position;
		Vector3 offset = targetPos - startPos;
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

		this.accelerationY = baseAccelerationY * Mathf.Pow(flowSpeed, 2f);

		speedY = (offset.y + this.accelerationY * Mathf.Pow(duration / flowSpeed, 2f) / 2) / (duration / flowSpeed);

		float linearTravelDistance = 0f;

		float previousAcceleration = this.accelerationY;
		while(linearTravelDistance < linearDistance)
		{
			float deltaTime = Time.deltaTime;

			linearTravelDistance += linearSpeed * deltaTime * flowSpeed;
			Vector3 linearOffset =  linearDirection * linearSpeed * deltaTime * flowSpeed;

			previousAcceleration = accelerationY;
			this.accelerationY = baseAccelerationY * Mathf.Pow(flowSpeed, 2f);

			float deltaDisY = speedY * deltaTime - 
				Mathf.Pow(deltaTime, 2f) * accelerationY / 2;

			if(rotateObject)
				owner.transform.rotation = Quaternion.LookRotation(
					new Vector3(
						vLinearSpeed.x * flowSpeed,
						speedY,
						vLinearSpeed.z * flowSpeed));

			owner.transform.position += linearOffset + Vector3.up * deltaDisY;

			if(this.accelerationY == previousAcceleration)
				speedY = speedY - accelerationY * deltaTime;
			else
				speedY = speedY * flowSpeed - this.accelerationY * deltaTime;

			yield return new WaitForFixedUpdate();
		}


		owner.transform.position = targetPos;
		yield return new WaitForFixedUpdate();
	}



	protected IEnumerator moveWithHeight(GameObject owner, Vector3 targetPos, float offsetY, float speed, bool rotateObject)
	{
		Vector3 startPos = owner.transform.position;
		Vector3 startPosXZ = startPos - Vector3.up * startPos.y;
		Vector3 targetPosXZ = targetPos - Vector3.up * targetPos.y;

		float peakY;

		if(targetPos.y > startPos.y)
			peakY = offsetY + targetPos.y;
		else
			peakY = offsetY + startPos.y;

		Vector3 linearDirection = (targetPosXZ - startPosXZ);
		linearDirection.Normalize();

		float linearDistance = Vector3.Distance(targetPosXZ, startPosXZ);

		Vector3 vLinearSpeed = linearDirection * speed;

		linearSpeed = speed;

		// get the duration
		float duration = linearDistance / linearSpeed;

		// calculate the gravity
		float riseDistance = peakY - startPos.y;
		float dropDistance = peakY - targetPos.y;

		baseAccelerationY = Mathf.Pow(
			Mathf.Sqrt(riseDistance * 2) / duration + Mathf.Sqrt(dropDistance * 2) / duration, 
			2f);

		this.accelerationY = baseAccelerationY * Mathf.Pow(flowSpeed, 2f);

		speedY = Mathf.Sqrt(2 * riseDistance * this.accelerationY);

		float linearTravelDistance = 0f;

		float previousAcceleration = this.accelerationY;
		while(linearTravelDistance < linearDistance)
		{
			float deltaTime = Time.deltaTime;

			linearTravelDistance += linearSpeed * deltaTime * flowSpeed;
			Vector3 linearOffset =  linearDirection * linearSpeed * deltaTime * flowSpeed;

			previousAcceleration = this.accelerationY;
			this.accelerationY = baseAccelerationY * Mathf.Pow(flowSpeed, 2f);

			float deltaDisY = speedY * deltaTime - 
				Mathf.Pow(deltaTime, 2f) * this.accelerationY / 2;

			if(rotateObject)
				owner.transform.rotation = Quaternion.LookRotation(
					new Vector3(
						vLinearSpeed.x * flowSpeed,
						speedY,
						vLinearSpeed.z * flowSpeed));

			owner.transform.position += linearOffset + Vector3.up * deltaDisY;

			if(this.accelerationY == previousAcceleration)
				speedY = speedY - this.accelerationY * deltaTime;
			else
				speedY = speedY * flowSpeed - this.accelerationY * deltaTime;

			yield return new WaitForFixedUpdate();
		}
			

		owner.transform.position = targetPos;
		yield return new WaitForFixedUpdate();
	}
#endregion
}
