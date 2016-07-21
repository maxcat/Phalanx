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
	
	public bool IsEmpty
	{
		get { return ownerID <= 0; }
	}
#endregion

#region Constructor
	public Command(uint ownerID)
	{
		this.ownerID = ownerID;
		isCommandFinished = false;
	}

	public Command()
	{
		this.ownerID = 0;
	}
#endregion
}
