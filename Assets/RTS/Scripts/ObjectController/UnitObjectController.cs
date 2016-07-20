using UnityEngine;			// for vector only
using System.Collections;
using System.Collections.Generic;

public class UnitObjectController : ObjectController {

#region Fields
	protected UnitData 				data;
#endregion

#region Getter and Setter
	public float Speed
	{
		get 
		{
			// TODO: calculate the speed form the data and the buff
			return 1f;
		}
	}
#endregion

#region Constructor
	public UnitObjectController(uint id) : base (id)
	{

	}
#endregion

#region Imlement Virtual Functions
	public override List<GameFlow> GenerateGameFlows(uint tag, List<Command> commandList)
	{
		return null;
	}
#endregion
}
