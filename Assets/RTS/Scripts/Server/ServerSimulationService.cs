using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerSimulationService : MonoBehaviour {

#region Fields
	[SerializeField] protected List<ClientService> 			clientList;
	protected List<ObjectController> 				objectControllerList;
	protected uint 							tag;
	protected CommandPool 						commandPool;
#endregion

#region Override MonoBehaviour
	// Use this for initialization
	IEnumerator Start () {

		commandPool = new CommandPool();
		objectControllerList = new List<ObjectController>();
		tag = 1;

		while(true)
		{
			TimeStep step = new TimeStep(tag);
			List<Command> commandList = commandPool.GetCommands(tag - 1);
			
			for(int i = 0; i < objectControllerList.Count; i ++)
			{
				ObjectController controller = objectControllerList[i];
				step.AddGameFlows(controller.GenerateGameFlows(tag, commandList));
			}

			for(int i = 0; i < clientList.Count; i ++)
			{
				ClientService client = clientList[i];
				client.OnReceiveTimeStep(step);
			}

			yield return new WaitForSeconds(TimeStep.TIME_STEP_DURATION);
			tag ++;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
#endregion

#region Event Listener
	public void OnReceiveCommands(Command command)
	{
		commandPool.AddCommand(tag, command);		
	}
#endregion
}
