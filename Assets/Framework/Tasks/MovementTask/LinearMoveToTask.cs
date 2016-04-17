using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LinearMoveToTask : Task {

	#region Fields
	protected float speed;
	#endregion

	#region Constructor
	public LinearMoveToTask(MonoBehaviour mono, GameObject owner, Vector3 targetPos, float duration)
		: base(mono)
	{
		coroutine = moveFlow(owner, targetPos, duration);
	}

	public LinearMoveToTask(MonoBehaviour mono, GameObject owner, float speed, Vector3 targetPos)
		:base(mono)
	{
		coroutine = moveFlow(owner, speed, targetPos);
	}

	#endregion

	#region Protected Functions
	protected IEnumerator moveFlow(GameObject owner, Vector3 targetPos, float duration)
	{
		Vector3 startPos = owner.transform.position;
		Vector3 direction = targetPos - startPos;
		Vector3 vSpeed = direction / duration;
		float distance = Vector3.Distance(targetPos, startPos);
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

	protected IEnumerator moveFlow(GameObject owner, float speed, Vector3 targetPos)
	{
		Vector3 startPos = owner.transform.position;
		Vector3 direction = targetPos - startPos;

		float distance = Vector3.Distance(targetPos, startPos);
		Vector3 speedDirection = direction.normalized;

		this.speed = speed;

		float traveledDistance = 0f;
		while(traveledDistance < distance)
		{
			float deltaTime = Time.deltaTime;
			traveledDistance += speed * deltaTime * taskSpeed;
			owner.transform.position += speedDirection * speed * deltaTime * taskSpeed;
			yield return new WaitForFixedUpdate();
		}
		owner.transform.position = targetPos;
		yield return new WaitForFixedUpdate();
	}
		
	#endregion
}
