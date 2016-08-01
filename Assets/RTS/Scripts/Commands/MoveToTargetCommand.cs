/*
using UnityEngine;
using System.Collections;

public class MoveToTargetCommand : Command {
#region Fields
	protected uint 				targetObjectID;
#endregion

#region Constructor
	public MoveToTargetCommand(uint tag, uint ownerID, uint targetObjID)
	       	: base (tag, ownerID)
	{
		this.targetObjectID = targetObjID;
	}
#endregion

#region Implement Virtual Functions
	public override void Execute(ObjectState currentState, ObjectState nextState)
	{
		Vector2 startPos = currentState.Positions[currentState.Positions.Count - 1];

		ObjectController targetCtrl = ObjectManager.Instance.GetObject(targetObjectID);
	}
#endregion
}
*/
