using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveToPosCommand : Command {

#region Fields
	protected Vector2 				destPos;
#endregion

#region Constructor
	public MoveToPosCommand(uint tag, uint ownerID, Vector2 destPos)
	       	: base (tag, ownerID)
	{
		this.destPos = destPos;
	}
#endregion

#region Implement Virtual Functions
	public override bool Execute(ObjectState currentState, ObjectState nextState)
	{
		// TODO: read speed from unit data.
		bool finishedInThisState = false;
		float speed = 10;
		Vector2 startPos = currentState.EndPos;	
		nextState.StartPos = startPos;

		Vector2 direction = destPos - startPos;
		float distance = direction.magnitude;
		direction.Normalize();

		if(distance > speed)
		{
			nextState.EndPos = speed * direction + startPos;
		}
		else
		{
			nextState.EndPos = destPos;
			finishedInThisState = true;
		}
		return finishedInThisState;
	}
#endregion
}
