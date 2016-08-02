using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectState {

#region Fields
	protected List<Command> 				commands;
	protected List<Vector2> 				positions;
	protected uint 						stateTag;
	protected bool 						isPrediction = false;
#endregion

#region Getter and Setter
	public List<Vector2> Positions
	{
		get { return positions; }
		set { positions = value; }
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
		positions = new List<Vector2>();
		stateTag = tag;
		isPrediction = false;
	}
#endregion

#region Public API
	public void AddPosition(Vector2 position)
	{
		positions.Add(position);
	}

	public void AddCommands(List<Command> commandList)
	{
		commands.AddRange(commandList);
	}
	
	public void AddCommand(Command command)
	{
		commands.Add(command);
	}

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
		newState.AddPosition(positions[positions.Count - 1]);

		ExecuteCommands(newState, newCommands);
		return newState;
	}
#endregion

#region Virtual Functions
	// TODO: test function, deep copy the current object state for client.
	public virtual ObjectState Deserialize()
	{
		ObjectState clonedState = new ObjectState(this.stateTag);
		clonedState.Positions = this.positions.Clone();

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
