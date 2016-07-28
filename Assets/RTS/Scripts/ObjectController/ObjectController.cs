using UnityEngine;			// for vector only
using System.Collections;
using System.Collections.Generic;

public class ObjectController {

#region Fields
	protected uint 						id;
	Dictionary<uint, ObjectState>				States;
#endregion

#region Getter and Setter
	public uint ID
	{
		get { return id; }
	}
#endregion

#region Constructor
	public ObjectController(uint id, uint createdTag)
	{
		this.id = id;
	}
#endregion

#region Virtual Functions
	public virtual void UpdateState(uint commandTag)
	{
	
	}

	public virtual ObjectState GetState(uint serverTag)
	{
		return null;
	}
#endregion
}
