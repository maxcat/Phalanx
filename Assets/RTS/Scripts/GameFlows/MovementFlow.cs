using UnityEngine;
using System.Collections;

public class MovementFlow : GameFlow {

#region Fields
	protected GameObject 			movingObj;
	protected GameObject 			targetObj = null;
	protected Vector3			destination;
	protected Vector3 			velocity;
#endregion

#region Getter and Setter 
	public bool HasTarget
	{
		get { return targetObj != null; }
	}	
#endregion

#region Constructor
	public MovementFlow(GameObject movingObj, Vector3 velocity, GameObject targetObj)
		: base ()
	{
		this.movingObj = movingObj;
		this.targetObj = targetObj;
		this.velocity = velocity;

		this.source = moveToTargetEnumerator();
	}

	public MovementFlow(GameObject movingObj, Vector3 velocity, Vector3 destination)
		: base ()
	{
		this.movingObj = movingObj;
		this.velocity = velocity;
		this.destination = destination;

		this.source = moveToDestEnumerator();
	}
#endregion

#region Sub Flow
	protected IEnumerator moveToDestEnumerator()
	{
		yield return null;
	}

	protected IEnumerator moveToTargetEnumerator()
	{
		yield return null;
	}
#endregion
}
