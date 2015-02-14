using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParabolaByTask : Task {

	#region Fields
	protected float m_linearSpeed;
	protected float m_accelerationY;
	protected float m_speedY;

	protected float m_baseAccelerationY;
	#endregion

	#region Constructor
	public ParabolaByTask(GameObject owner, float speed, float accelerationY, Vector3 offset, Vector3 fixSpeedDirection,  bool rotateObject = false)
		: base ()
	{
		m_coroutine = moveFlow(owner, speed, accelerationY, offset, fixSpeedDirection, rotateObject);
	}
	#endregion

	#region Protected Functions
	protected IEnumerator moveFlow(GameObject owner, float speed, float accelerationY, Vector3 offset, Vector3 fixSpeedDirection, bool rotateObject)
	{
		Vector3 startPos = owner.transform.position;
		Vector3 targetPos = startPos + offset;
		Vector3 offsetXZ = offset - Vector3.up * offset.y;


		float duration;
		Vector3 vSpeed;
		float actualSpeed;

		if(fixSpeedDirection == Vector3.right)
		{
			duration = Mathf.Abs(offset.x) / Mathf.Abs(speed);
			vSpeed = new Vector3(speed, 0f, offset.z / duration);
			actualSpeed = vSpeed.magnitude;
		}
		else if(fixSpeedDirection == Vector3.forward)
		{
			duration = Mathf.Abs(offset.z) / Mathf.Abs(speed);
			vSpeed = new Vector3(offset.x / duration, 0f, speed);
			actualSpeed = vSpeed.magnitude;
		}
		else
		{
			duration = offsetXZ.magnitude / Mathf.Abs(speed);
			actualSpeed = speed;
		}

		Vector3 linearDirection = offsetXZ;
		linearDirection.Normalize();

		float linearDistance = offsetXZ.magnitude;

		Vector3 linearSpeed = offsetXZ / duration;

		m_linearSpeed = linearSpeed.magnitude;

		m_baseAccelerationY = accelerationY;

		m_accelerationY = m_baseAccelerationY * Mathf.Pow(m_taskSpeed, 2f);

		m_speedY = (offset.y + m_accelerationY * Mathf.Pow(duration / m_taskSpeed, 2f) / 2) / (duration / m_taskSpeed);

		float linearTravelDistance = 0f;

		float previousAcceleration = m_accelerationY;
		while(linearTravelDistance < linearDistance)
		{
			float deltaTime = Time.deltaTime;

			linearTravelDistance += m_linearSpeed * deltaTime * m_taskSpeed;
			Vector3 linearOffset =  linearDirection * m_linearSpeed * deltaTime * m_taskSpeed;

			previousAcceleration = m_accelerationY;
			m_accelerationY = m_baseAccelerationY * Mathf.Pow(m_taskSpeed, 2f);

			float deltaDisY = m_speedY * deltaTime - 
				Mathf.Pow(deltaTime, 2f) * m_accelerationY / 2;

			if(rotateObject)
				owner.transform.rotation = Quaternion.LookRotation(
					new Vector3(
						linearSpeed.x * m_taskSpeed,
						m_speedY,
						linearSpeed.z * m_taskSpeed));

			owner.transform.position += linearOffset + Vector3.up * deltaDisY;

			if(m_accelerationY == previousAcceleration)
				m_speedY = m_speedY - m_accelerationY * deltaTime;
			else
				m_speedY = m_speedY * m_taskSpeed - m_accelerationY * deltaTime;

			yield return new WaitForFixedUpdate();
		}


		owner.transform.position = targetPos;
		yield return new WaitForFixedUpdate();
	}
		
	#endregion

}
