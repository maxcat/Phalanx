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

		uint previousStateTag = commandTag + commandDelayInStep * (uint)TimeStep.STATES_PER_TIME_STEP - 1;
		uint currentStateTag = previousStateTag + 1;

		if(states.ContainsKey(previousStateTag))
		{
			for(int i = 0; i < TimeStep.STATES_PER_TIME_STEP; i ++ )
			{
				ObjectState nextState = null;
				if(i == 0)
					nextState = states[previousStateTag].GenerateNextState(objectCommands);
				else
					nextState = states[previousStateTag].GenerateNextState(null);


				if(states.ContainsKey(currentStateTag))
					states[currentStateTag] = nextState;
				else
					states.Add(currentStateTag, nextState);

				previousStateTag ++;
				currentStateTag ++;
			}
		}
	}

	public virtual ObjectState GetState(uint stateTag)
	{
		if(states.ContainsKey(stateTag))
		{
			return states[stateTag];
		}
		else
		{
			return null;
		}
	}

	public virtual void Init(uint tag, Vector2 startPos)
	{
		states = new Dictionary<uint, ObjectState>(); 

		for(int i = 0; i < TimeStep.STATES_PER_TIME_STEP; i ++)
		{
			uint stateTag = tag + (uint)i;
			ObjectState startState = new ObjectState(stateTag);
			startState.StartPos = startPos;
			startState.EndPos = startPos;

			states.Add(stateTag, startState);
		}
	}
#endregion
}
