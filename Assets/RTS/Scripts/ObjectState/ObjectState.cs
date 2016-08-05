using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectState {

#region Static Fields
	public static readonly float 				DURATION = 0.2f;
#endregion

#region Fields
	protected List<CommandSnapshot> 			commandSnapshots;
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

	public List<CommandSnapshot> PassOverCommands
	{
		get
		{
			return commandSnapshots.FindAll(command => !command.FinishedInThisState);
		}
	}

	public List<CommandSnapshot> CommandSnapshots
	{
		get { return commandSnapshots; }
		set { commandSnapshots = value; }
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
		commandSnapshots = new List<CommandSnapshot>();
		stateTag = tag;
		isPrediction = false;
	}
#endregion

#region Public API
	public ObjectState GenerateNextState(List<Command> commandList)
	{
		ObjectState newState = new ObjectState(stateTag + 1);	

		List<CommandSnapshot> snapshotList = new List<CommandSnapshot>();
		snapshotList.AddRange(takeCommandSnapshots(this.PassOverCommands));

		if(commandList != null)
		{ 
			snapshotList.AddRange(takeCommandSnapshots(commandList));
		}

		newState.CommandSnapshots = snapshotList;
		newState.StartPos = this.endPos;
		newState.EndPos = this.endPos;

		ExecuteCommands(newState, snapshotList);
		return newState;
	}
#endregion

#region Virtual Functions
	protected virtual void ExecuteCommands(ObjectState nextState, List<CommandSnapshot> snapshotList)
	{
		for(int i = 0; i < snapshotList.Count; i ++)
		{
			snapshotList[i].Execute(this, nextState);
		}
	}
#endregion

#region Protected Functions
	protected List<CommandSnapshot> takeCommandSnapshots(List<Command> commandList)
	{
		List<CommandSnapshot> snapshots = new List<CommandSnapshot>();	

		if(commandList != null)
		{
			for (int i = 0; i < commandList.Count; i ++)
			{
				snapshots.Add(new CommandSnapshot(commandList[i]));
			}
		}
		return snapshots;
	}

	protected List<CommandSnapshot> takeCommandSnapshots(List<CommandSnapshot> snapshotList)
	{
		List<CommandSnapshot> snapshots = new List<CommandSnapshot>();	

		if(snapshotList != null)
		{
			for (int i = 0; i < snapshotList.Count; i ++)
			{
				snapshots.Add(new CommandSnapshot(snapshotList[i].Base));
			}
		}
		return snapshots;
	}
#endregion
}
