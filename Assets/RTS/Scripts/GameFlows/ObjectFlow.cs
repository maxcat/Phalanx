using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectFlow : Flow {

#region Fields
	protected Dictionary<uint, ObjectState>		states;
	protected GameObject 				owner;
	protected uint 					stateTag;
#endregion

#region Constructor
	public ObjectFlow(GameObject owner, Dictionary<uint, ObjectState> states, uint startTag)
		: base ()
	{
		this.owner = owner;
		this.states = states;
		this.stateTag = startTag;
	}
#endregion

}
