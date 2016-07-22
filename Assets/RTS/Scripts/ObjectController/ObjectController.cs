using UnityEngine;			// for vector only
using System.Collections;
using System.Collections.Generic;

public class ObjectController {

#region Fields
	protected uint 						id;
	protected Vector2 					currentPos;
	protected uint 						currentTag;
#endregion

#region Getter and Setter
	public Vector2 CurrentPos
	{
		get { return currentPos; }
	}

	public uint ID
	{
		get { return id; }
	}
#endregion

#region Constructor
	public ObjectController(uint id)
	{
		this.id = id;
	}
#endregion

#region Virtual Functions
	public virtual List<GameFlow> GenerateGameFlows(uint tag, List<Command> commandList)
	{
		return null;
	}
#endregion
}
