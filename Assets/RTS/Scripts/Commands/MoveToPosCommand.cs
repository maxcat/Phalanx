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
	public override Command Deserialize()
	{
		MoveToPosCommand clonedCommand = new MoveToPosCommand(this.sendTag, this.ownerID, this.destPos);

		clonedCommand.FinishInThisStep = this.finishInThisStep;

		return clonedCommand;
	}

	public override void Execute(ObjectState currentState, ObjectState nextState)
	{
		// TODO: read speed from unit data.
		float speed = 10;
		Vector2 startPos = currentState.Positions[currentState.Positions.Count - 1];	

		Vector2 direction = destPos - startPos;
		float distance = direction.magnitude;
		direction.Normalize();

		// reset the position of the next state.
		nextState.Positions = new List<Vector2>(); 

		// reserve one more calculation for the next state
		for (int i = 0; i <= TimeStep.MOVEMENTS_PER_STEP; i ++)
		{
			Vector2 pos = destPos;
			if(distance > i * speed)
			{
				pos = speed * direction * i + startPos;
			}
			else
			{
				finishInThisStep = true;
			}

			nextState.Positions.Add(pos);
		}
	}
#endregion
}
