using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommandManager {

#region Static Fields
	private static CommandManager 					instance;
#endregion

#region Static Functions
	public static CommandManager Instance
	{
		get 
		{
			if(instance == null)
				instance = new CommandManager();
			return instance;
		}
	}
#endregion


#region Fields
	protected Dictionary<uint, List<Command>>  			commandPool;	
	protected List<Command> 					receivedCommandList;
#endregion

#region Constructor
	public CommandManager()
	{
		this.commandPool = new Dictionary<uint, List<Command>>();
		this.receivedCommandList = new List<Command>();
	}
#endregion

#region Public API
	public void OnReceiveCommand(Command command)
	{
		receivedCommandList.Add(command);
	}

	public uint MoveReceivedCommandToPool()
	{
		uint oldestCommandTag = 0;
		if(receivedCommandList.Count > 0)
		{
			for(int i = 0; i < receivedCommandList.Count; i ++)
			{
				Command command = receivedCommandList[i];
				if(!commandPool.ContainsKey(command.SendTag))
					commandPool[command.SendTag] = new List<Command>();

				commandPool[command.SendTag].Add(command);

				if(oldestCommandTag == 0)
				{
					oldestCommandTag = command.SendTag;
				}
				else if(oldestCommandTag > command.SendTag)
				{
					oldestCommandTag = command.SendTag;
				}
			}
			receivedCommandList.Clear();
		}
		return oldestCommandTag;
	}

	// TODO: sort command with priority
	public List<Command> GetCommands(uint timeTag)
	{
		if(commandPool.ContainsKey(timeTag))
			return commandPool[timeTag];

		return null;
	}

	public List<Command> GetCommands(uint timeTag, uint ownerID)
	{
		if(commandPool.ContainsKey(timeTag))
		{
			List<Command> result = commandPool[timeTag];
			return result.FindAll(command => command.OwnerID == ownerID);
		}

		return null;
	}
#endregion

}
