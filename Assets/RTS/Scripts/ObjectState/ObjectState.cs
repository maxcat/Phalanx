using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectState {

#region Fields
	protected List<Vector2> 				objectPositions;
	protected List<Command> 				commands;
#endregion

#region Getter and Setter
	public List<Vector2> Positions
	{
		get { return objectPositions; }
	}
#endregion

#region Public API
	public virtual ObjectState GenerateNextState(List<Command> commandList)
	{
		return null;
	}
#endregion

}
