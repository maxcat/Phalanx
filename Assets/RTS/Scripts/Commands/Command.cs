using UnityEngine;
using System.Collections;

public class Command {

#region Fields
	protected uint 		ownerID;
	protected bool 		isCommandFinished = false;
#endregion

#region Getter and Setter
	public bool IsFinished
	{
		get { return isCommandFinished; }
		set { isCommandFinished = value; }
	}

	public uint OwnerID
	{
		get { return ownerID; }
	}
#endregion

#region Constructor
	public Command(uint ownerID)
	{
		this.ownerID = ownerID;
		isCommandFinished = false;
	}
#endregion
}
