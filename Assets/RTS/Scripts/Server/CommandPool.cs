using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommandPool {

#region Fields
	protected Dictionary<uint, List<Command>>  			commandPool;	
#endregion

#region Constructor
	public CommandPool()
	{
		commandPool = new Dictionary<uint, List<Command>>();
	}
#endregion

#region Public API
	public void AddCommand(uint tag, Command command)
	{
		if(commandPool.ContainsKey(tag))
		{
			commandPool[tag].Add(command);
		}
		else
		{
			List<Command> commandList = new List<Command>();
			commandList.Add(command);
			commandPool.Add(tag, commandList);
		}
	}

	public List<Command> GetCommands(uint tag)
	{
		if(commandPool.ContainsKey(tag))
		{
			return commandPool[tag];
		}	
		else if(tag >= 0)
		{
			List<Command> result = new List<Command>();
			result.Add(new Command());
			return result;
		}
		return null;
	}
#endregion

}
