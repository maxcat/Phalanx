using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerSimulationService : MonoBehaviour {

#region Fields
	[SerializeField] protected List<ClientService> 			clientList;
	[SerializeField] protected uint 				commandDelayInStep = 1;
	[SerializeField] protected uint 				maxCommandStepDelay = 3;

	protected List<ObjectController> 				objectControllerList;
	protected uint 							serverTag;
	protected CommandPool 						commandPool;
#endregion

#region Getter and Setter
	public CommandPool Commands
	{
		get { return commandPool; }
	}
#endregion

#region Override MonoBehaviour
	// Use this for initialization
	IEnumerator Start () {

		commandPool = new CommandPool();
		objectControllerList = new List<ObjectController>();
		serverTag = 1;

		while(true)
		{
			TimeStep step = new TimeStep(serverTag);
			List<Command> commandList = commandPool.GetCommands(serverTag - commandDelayInStep);
			
			for(int i = 0; i < objectControllerList.Count; i ++)
			{
				ObjectController controller = objectControllerList[i];
				step.AddGameFlows(controller.GenerateGameFlows(serverTag, commandList));
			}

			for(int i = 0; i < clientList.Count; i ++)
			{
				ClientService client = clientList[i];
				client.OnReceiveTimeStep(step);
			}

			yield return new WaitForSeconds(TimeStep.TIME_STEP_DURATION);
			serverTag++;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
#endregion

#region Event Listener
	public void OnReceiveCommands(Command command)
	{
		commandPool.AddCommand(serverTag, command);		
	}
#endregion

#region Public API
	public void AddObjectController(ObjectController controller)
	{
		objectControllerList.Add(controller);
	}
#endregion
}
