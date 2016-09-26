using HRGameLogic;			
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
	public virtual void UpdateState(uint tag, uint commandDelayInState)
	{
		List<Command> objectCommands = new List<Command>();
		if(tag > commandDelayInState)
		{
			uint commandTag = tag - commandDelayInState;
			objectCommands = CommandManager.Instance.GetCommands(commandTag, id);
		}	

		uint previousStateTag = tag - 1;
		if(states.ContainsKey(previousStateTag))
		{
			ObjectState nextState = null;
			nextState = states[previousStateTag].GenerateNextState(objectCommands);

			if(states.ContainsKey(tag))
				states[tag] = nextState;
			else
				states.Add(tag, nextState);
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

	public virtual void Init(uint tag, HRVector2D startPos)
	{
		states = new Dictionary<uint, ObjectState>(); 

		uint stateTag = tag;
		ObjectState startState = new ObjectState(stateTag);
		startState.StartPos = startPos;
		startState.EndPos = startPos;

		states.Add(stateTag, startState);
	}
#endregion
}
