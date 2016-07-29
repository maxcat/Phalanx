using UnityEngine;			// for vector only
using System.Collections;
using System.Collections.Generic;

public class ObjectController {

#region Fields
	protected uint 						id;
	Dictionary<uint, ObjectState>				states;
#endregion

#region Getter and Setter
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
	public virtual void UpdateState(uint commandTag, uint commandDelayInStep)
	{
		List<Command> objectCommands = CommandManager.Instance.GetCommands(commandTag, id);

		uint previousStateTag = commandTag + commandDelayInStep - 1;
		uint currentStateTag = previousStateTag + 1;

		if(states.ContainsKey(previousStateTag))
		{
			ObjectState nextState = states[previousStateTag].GenerateNextState(objectCommands);
			if(states.ContainsKey(currentStateTag))
				states[currentStateTag] = nextState;
			else
				states.Add(currentStateTag, nextState);
		}
	}

	public virtual ObjectState GetState(uint serverTag)
	{
		return null;
	}

	public virtual void Init(uint tag, Vector2 startPos)
	{
		states = new Dictionary<uint, ObjectState>(); 

		ObjectState startState = new ObjectState(tag);
		startState.Positions.Add(startPos);

		states.Add(tag, startState);
	}
#endregion
}
