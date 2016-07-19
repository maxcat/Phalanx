using UnityEngine;
using System.Collections;

public class MovementFlow : GameFlow {

#region Fields
	protected GameObject 			movingObj;
	protected GameObject 			targetObj = null;
	protected Vector3			velocity;
	protected float 			speed;
	protected Vector3 			startPos;
#endregion

#region Getter and Setter 
	public bool HasTarget
	{
		get { return targetObj != null; }
	}	
#endregion

#region Constructor
	public MovementFlow(GameObject movingObj, Vector2 startPos, float speed, GameObject targetObj)
		: base ()
	{
		this.movingObj = movingObj;
		this.targetObj = targetObj;
		this.startPos = (Vector3)startPos; 
		this.speed = speed;

		this.source = moveToTargetEnumerator();
	}

	public MovementFlow(GameObject movingObj, Vector2 startPos, Vector2 velocity)
		: base ()
	{
		this.movingObj = movingObj;
		this.velocity = (Vector3)velocity;
		this.startPos = (Vector3)startPos;

		this.source = moveToPosEnumerator();
	}
#endregion

#region Sub Flow
	protected IEnumerator moveToPosEnumerator()
	{
		// TODO: smooth translation to the start postion
		movingObj.transform.localPosition = startPos;

		float timeElapse = 0f; 
		while(timeElapse < TimeStep.TIME_STEP_DURATION)
		{
			float deltaTime = Time.deltaTime;
			timeElapse += deltaTime;
			yield return null;
			movingObj.transform.localPosition += velocity * deltaTime;
		}
	}

	protected IEnumerator moveToTargetEnumerator()
	{
		// TODO: smooth translation to the start postion
		movingObj.transform.localPosition = startPos;

		float timeElapse = 0f;
		while(timeElapse < TimeStep.TIME_STEP_DURATION)
		{
			float deltaTime = Time.deltaTime;
			timeElapse += deltaTime;
			yield return null;

			if(Vector3.Distance(targetObj.transform.localPosition, movingObj.transform.localPosition) > 1f)
			{
				Vector3 offset = (targetObj.transform.localPosition - movingObj.transform.localPosition).normalized * speed * deltaTime;
				targetObj.transform.localPosition += offset;
			}

		}
		yield return null;
	}
#endregion
}
