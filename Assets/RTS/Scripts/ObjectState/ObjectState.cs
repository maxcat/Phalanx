using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectState {

#region Static Fields
	public static readonly float 				DURATION = 0.2f;
#endregion

#region Fields
	protected List<Command> 				commands;
	protected Vector2 					startPos;
	protected Vector2 					endPos;
	protected uint 						stateTag;
	protected bool 						isPrediction = false;
#endregion

#region Getter and Setter
	public Vector2 StartPos
	{
		get { return startPos; }
		set { startPos = value; }
	}

	public Vector2 EndPos
	{
		get { return endPos; }
		set { endPos = value; }
	}

	public List<Command> PassOverCommands
	{
		get
		{
			return commands.FindAll(command => !command.FinishInThisStep);
		}
	}

	public List<Command> Commands
	{
		get { return commands; }
		set { commands = value; }
	}

	public uint StateTag
	{
		get { return stateTag; }
	}

	public bool IsPrediction
	{
		get { return isPrediction; }
		set { isPrediction = value; }
	}
#endregion

#region Constructor
	public ObjectState(uint tag)
	{
		commands = new List<Command>();
		stateTag = tag;
		isPrediction = false;
	}
#endregion

#region Public API
	public ObjectState GenerateNextState(List<Command> commandList)
	{
		ObjectState newState = new ObjectState(stateTag + 1);	

		List<Command> newCommands = new List<Command>();
		newCommands.AddRange(cloneCommandList(this.PassOverCommands));

		if(commandList != null)
		{ 
			newCommands.AddRange(cloneCommandList(commandList));
		}

		newState.Commands = newCommands;
		newState.StartPos = this.endPos;
		newState.EndPos = this.endPos;

		ExecuteCommands(newState, newCommands);
		return newState;
	}
#endregion

#region Virtual Functions
	// TODO: test function, deep copy the current object state for client.
	public virtual ObjectState Deserialize()
	{
		ObjectState clonedState = new ObjectState(this.stateTag);
		clonedState.StartPos = this.startPos;
		clonedState.EndPos = this.endPos;

		List<Command> commandList = new List<Command>();
		for(int i = 0; i < commands.Count; i ++)
		{
			commandList.Add(commands[i].Deserialize());	
		}
		clonedState.Commands = commandList;

		clonedState.IsPrediction = this.isPrediction;	
		return clonedState;
	}

	protected virtual void ExecuteCommands(ObjectState nextState, List<Command> commandList)
	{
		for(int i = 0; i < commandList.Count; i ++)
		{
			commandList[i].Execute(this, nextState);
		}
	}
#endregion

#region Protected Functions
	protected List<Command> cloneCommandList(List<Command> commandList)
	{
		List<Command> result = new List<Command>();

		if(commandList != null)
		{
			for (int i = 0; i < commandList.Count; i ++)
			{
				result.Add(commandList[i].Deserialize());
			}
		}
		return result;
	}
#endregion
}
