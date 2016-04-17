using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LinearMoveByTask : Task {

	#region Fields
	protected float speed;
	#endregion

	#region Constructor
	public LinearMoveByTask(MonoBehaviour mono, GameObject owner, Vector3 offset, float duration)
		: base(mono)
	{
		coroutine = moveFlow(owner, offset, duration);
	}

	public LinearMoveByTask(MonoBehaviour mono, GameObject owner, float speed, Vector3 offset, Vector3 fixSpeedDirection)
		:base(mono)
	{
		coroutine = moveFlow(owner, speed, offset, fixSpeedDirection);
	}

	#endregion

	#region Protected Functions
	protected IEnumerator moveFlow(GameObject owner, Vector3 offset, float duration)
	{
		Vector3 startPos = owner.transform.position;
		Vector3 targetPos = startPos + offset;
		Vector3 direction = offset;
		Vector3 vSpeed = direction / duration;
		float distance = offset.magnitude;
		Vector3 speedDirection = vSpeed.normalized;

		speed = Vector3.Distance(vSpeed, Vector3.zero);

		float traveledDistance = 0f;
		while(traveledDistance < distance)
		{
			float deltaTime = Time.deltaTime;
			traveledDistance += speed * deltaTime * taskSpeed;
			owner.transform.position += speedDirection * speed * deltaTime * taskSpeed;
			yield return new WaitForFixedUpdate();
		}

		// reset pos
		owner.transform.position = targetPos;
		yield return new WaitForFixedUpdate();
	}

	protected IEnumerator moveFlow(GameObject owner, float speed, Vector3 offset, Vector3 fixSpeedDirection)
	{
		Vector3 startPos = owner.transform.position;
		Vector3 direction = offset;
		Vector3 targetPos = startPos + offset;

		float distance = offset.magnitude;
		Vector3 speedDirection = direction.normalized;

		float duration;
		Vector3 vSpeed;

		if(fixSpeedDirection == Vector3.up)
		{
			duration = Mathf.Abs(offset.y) / Mathf.Abs(speed);
			vSpeed = new Vector3(offset.x / duration, speed, offset.z / duration);
			this.speed = vSpeed.magnitude;
		}
		else if(fixSpeedDirection == Vector3.right)
		{
			duration = Mathf.Abs(offset.x) / Mathf.Abs(speed);
			vSpeed = new Vector3(speed, offset.y / duration, offset.z / duration);
			this.speed = vSpeed.magnitude;
		}
		else if(fixSpeedDirection == Vector3.forward)
		{
			duration = Mathf.Abs(offset.z) / Mathf.Abs(speed);
			vSpeed = new Vector3(offset.x / duration, offset.y / duration, speed);
			this.speed = vSpeed.magnitude;
		}
		else
		{
			this.speed = speed;
		}


		float traveledDistance = 0f;
		while(traveledDistance < distance)
		{
			float deltaTime = Time.deltaTime;
			traveledDistance += speed * deltaTime * taskSpeed;
			owner.transform.position += speedDirection * this.speed * deltaTime * taskSpeed;
			yield return new WaitForFixedUpdate();
		}
		owner.transform.position = targetPos;
		yield return new WaitForFixedUpdate();
	}

	#endregion
}
