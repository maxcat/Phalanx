using UnityEngine;
using System.Collections;

public class MovementCommand : Command {

#region Fields
	protected uint 			targetObjectID = 0;
	protected Vector2 		destPos;
#endregion

#region Getter and Setter
	public bool IsMoveToDest
	{
		get { return targetObjectID <= 0; }
	}
#endregion

#region Constructor
	public MovementCommand(uint ownerID, uint targetObjID) : base (ownerID)
	{
		this.targetObjectID = targetObjID;
	}
	
	public MovementCommand(uint ownerID, Vector2 destPos) : base (ownerID)
	{
		this.destPos = destPos;
		this.targetObjectID = 0;
	}
#endregion
}
