using UnityEngine;
using System.Collections;

public class LinearMoveToFlow : Flow {

#region Fields
	protected float 		speed;
#endregion 

#region Constructor
	public LinearMoveToFlow(GameObject owner, Vector3 targetPos, float duration) : base ()
	{
		source = moveWithDuration(owner, targetPos, duration);
	}

	public LinearMoveToFlow(GameObject owner, float speed, Vector3 targetPos) : base ()
	{
		source = moveWithSpeed(owner, speed, targetPos);
	}
#endregion

#region Protected Functions
	protected IEnumerator moveWithDuration(GameObject owner, Vector3 targetPos, float duration)
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
			traveledDistance += speed * deltaTime * flowSpeed;
			owner.transform.position += speedDirection * speed * deltaTime * flowSpeed;
			yield return new WaitForFixedUpdate();
		}

		// reset pos
		owner.transform.position = targetPos;
		yield return new WaitForFixedUpdate();
	}

	protected IEnumerator moveWithSpeed(GameObject owner, float speed, Vector3 targetPos)
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
			traveledDistance += speed * deltaTime * flowSpeed;
			owner.transform.position += speedDirection * speed * deltaTime * flowSpeed;
			yield return new WaitForFixedUpdate();
		}
		owner.transform.position = targetPos;
		yield return new WaitForFixedUpdate();
	}
		
#endregion
}
