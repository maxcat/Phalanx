using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LinearMoveToTask : Task {

	#region Fields
	protected float m_speed;
	#endregion

	#region Constructor
	public LinearMoveToTask(GameObject owner, Vector3 targetPos, float duration)
		: base()
	{
		m_coroutine = moveFlow(owner, targetPos, duration);
	}

	public LinearMoveToTask(GameObject owner, float speed, Vector3 targetPos)
		:base()
	{
		m_coroutine = moveFlow(owner, speed, targetPos);
	}

	#endregion

	#region Protected Functions
	protected IEnumerator moveFlow(GameObject owner, Vector3 targetPos, float duration)
	{
		Vector3 startPos = owner.transform.position;
		Vector3 direction = targetPos - startPos;
		Vector3 speed = direction / duration;
		float distance = Vector3.Distance(targetPos, startPos);
		Vector3 speedDirection = speed.normalized;

		m_speed = Vector3.Distance(speed, Vector3.zero);

		float traveledDistance = 0f;
		while(traveledDistance < distance)
		{
			float deltaTime = Time.deltaTime;
			traveledDistance += m_speed * deltaTime * m_taskSpeed;
			owner.transform.position += speedDirection * m_speed * deltaTime * m_taskSpeed;
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

		m_speed = speed;

		float traveledDistance = 0f;
		while(traveledDistance < distance)
		{
			float deltaTime = Time.deltaTime;
			traveledDistance += m_speed * deltaTime * m_taskSpeed;
			owner.transform.position += speedDirection * m_speed * deltaTime * m_taskSpeed;
			yield return new WaitForFixedUpdate();
		}
		owner.transform.position = targetPos;
		yield return new WaitForFixedUpdate();
	}
		
	#endregion
}
