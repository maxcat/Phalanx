using UnityEngine;
using System.Collections;

public class Command {

#region Fields
	protected uint 		ownerID;
	protected uint 		sendTag;
	protected bool 		finishInThisStep = false;
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

	public bool FinishInThisStep
	{
		get { return finishInThisStep; }
		set { finishInThisStep = value; }
	}
#endregion

#region Constructor
	public Command(uint tag, uint ownerID)
	{
		this.ownerID = ownerID;
		this.sendTag = tag;
		this.finishInThisStep = false;
	}
#endregion

#region Server Side Virtual Functions
	public virtual void Execute(ObjectState currentState, ObjectState nextState)
	{
		
	}
#endregion
}
