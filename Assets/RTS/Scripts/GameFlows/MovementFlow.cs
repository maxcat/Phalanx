using UnityEngine;
using System.Collections;

public class MovementFlow : GameFlow {

#region Fields
	protected uint	 			movingObjID;
	protected uint	 			targetObjID = 0;
	protected float 			speed;
	protected Vector2 			startPos;
	protected Vector2 			destPos;
#endregion

#region Getter and Setter 
	public bool HasTarget
	{
		get { return targetObjID > 0; }
	}	
#endregion

#region Constructor
	public MovementFlow(uint movingObjID, Vector2 startPos, float speed, uint targetObjID)
		: base ()
	{
		this.movingObjID = movingObjID;
		this.targetObjID = targetObjID;
		this.startPos = startPos; 
		this.speed = speed;

		this.source = moveToTargetEnumerator();
	}

	public MovementFlow(uint movingObjID, Vector2 startPos, float speed, Vector2 destPos)
		: base ()
	{
		this.movingObjID = movingObjID;
		this.startPos = startPos;
		this.destPos = startPos;
		this.speed = speed;

		this.source = moveToPosEnumerator();
	}
#endregion

#region Sub Flow
	protected IEnumerator moveToPosEnumerator()
	{
		GameObject movingObj = clientService.GetObject(movingObjID);
		// TODO: smooth translation to the start postion
		movingObj.transform.localPosition = startPos;

		float timeElapse = 0f; 
		Vector2 velocity = (destPos - startPos).normalized * speed;
		while(timeElapse < TimeStep.TIME_STEP_DURATION)
		{
			float deltaTime = Time.deltaTime;
			timeElapse += deltaTime;
			yield return null;
			if(Vector3.Distance(movingObj.transform.localPosition, destPos) > 1f)
			{
				movingObj.transform.localPosition += (Vector3)velocity * deltaTime;
			}
		}
	}

	protected IEnumerator moveToTargetEnumerator()
	{
		GameObject movingObj = clientService.GetObject(movingObjID);
		GameObject targetObj = clientService.GetObject(targetObjID);
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
