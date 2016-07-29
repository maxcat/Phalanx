using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectState {

#region Fields
	protected List<Command> 				commands;
	protected List<Vector2> 				positions;
	protected uint 						stateTag;
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
#endregion

#region Constructor
	public ObjectState(uint tag)
	{
		commands = new List<Command>();
		positions = new List<Vector2>();
		stateTag = tag;
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
		newCommands.AddRange(this.PassOverCommands);
		newCommands.AddRange(commandList);
		newState.Commands = newCommands;
		newState.AddPosition(positions[positions.Count - 1]);

		ExecuteCommands(newState, newCommands);
		return newState;
	}
#endregion

#region Virtual Functions
	protected virtual void ExecuteCommands(ObjectState nextState, List<Command> commandList)
	{
		for(int i = 0; i < commandList.Count; i ++)
		{
			commandList[i].Execute(this, nextState);
		}
	}
#endregion

}
