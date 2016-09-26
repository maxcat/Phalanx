using UnityEngine;
using System.Collections;

public class Command {

#region Fields
	protected uint 		ownerID;
	protected uint 		sendTag;
#endregion

#region Getter and Setter
	public uint OwnerID
	{
		get { return ownerID; }
	}
	
	public uint SendTag 
	{
		get { return sendTag; }
	}
#endregion

#region Constructor
	public Command(uint tag, uint ownerID)
	{
		this.ownerID = ownerID;
		this.sendTag = tag;
	}
#endregion

#region Server Side Virtual Functions
	public virtual bool Execute(ObjectState currentState, ObjectState nextState)
	{
		return true;	
	}
#endregion
}
