using UnityEngine;
using System.Collections;

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
	public override void Execute(ObjectState currentState, ObjectState nextState)
	{
		float speed = 10;
		Vector2 startPos = currentState.Positions[currentState.Positions.Count - 1];	

		Vector2 direction = destPos - startPos;
		float distance = direction.magnitude;
		direction.Normalize();

		for (int i = 0; i < TimeStep.MOVEMENTS_PER_STEP; i ++)
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
