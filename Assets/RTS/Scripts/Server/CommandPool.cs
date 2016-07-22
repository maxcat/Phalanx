using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommandPool {

#region Fields
	protected Dictionary<uint, List<Command>>  			commandPool;	
	protected uint 							commandHandleDelay;
	protected uint 							maxCommandHandleDelay;
#endregion

#region Constructor
	public CommandPool(uint commandHandleDelay, uint maxCommandHandleDelay)
	{
		this.commandPool = new Dictionary<uint, List<Command>>();
		this.maxCommandHandleDelay = maxCommandHandleDelay;
		this.commandHandleDelay = commandHandleDelay;
	}
#endregion

#region Public API
	public void AddCommand(uint receiveTag, Command command)
	{
		if(commandPool.ContainsKey(receiveTag))
		{
			commandPool[receiveTag].Add(command);
		}
		else
		{
			List<Command> commandList = new List<Command>();
			commandList.Add(command);
			commandPool.Add(receiveTag, commandList);
		}
	}

	public List<Command> GetCommands(uint serverTag)
	{
		uint tag = serverTag - commandHandleDelay;
		if(commandPool.ContainsKey(tag))
		{
			List<Command> result = commandPool[tag];
			return result;
		}	
		return null;
	}

	public List<Command> GetInvalidCommands(uint maxCommandDelay)
	{
		return null;
	}
#endregion

}
