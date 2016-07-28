using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerSimulationService : MonoBehaviour {

#region Fields
	[SerializeField] protected List<ClientService> 			clientList;
	[SerializeField] protected uint 				commandDelayInStep = 1;
	[SerializeField] protected uint 				maxCommandStepDelay = 3;

	protected uint 							serverTag;
#endregion

#region Override MonoBehaviour
	// Use this for initialization
	IEnumerator Start () {

		serverTag = 1;

		while(true)
		{
			TimeStep step = new TimeStep(serverTag);

			uint startTag = CommandManager.Instance.MoveReceivedCommandToPool();

			uint commandTag = serverTag - commandDelayInStep;
			if(commandTag >= 1)
			{
				if(startTag > commandTag)
					startTag = commandTag;
				else if(commandTag - startTag > maxCommandStepDelay)
					startTag = commandTag - maxCommandStepDelay;

				for(uint i = startTag; i <= commandTag; i ++)
				{
					List<Command> commandList = CommandManager.Instance.GetCommands(i);
					// execute commands
					// TODO: implement command priority
					if(commandList != null)
					{
						for(int j = 0; j < commandList.Count; j ++)
						{
							commandList[j].Execute();
						}
					}
					// update object states
					ObjectManager.Instance.UpdateState(i);
				}
			}
			
			// generate the next state
			List<ObjectState> stateList = ObjectManager.Instance.GetStates(serverTag);

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
		CommandManager.Instance.OnReceiveCommand(command);
	}
#endregion

#region Public API
	public void AddObjectController(ObjectController controller)
	{
		ObjectManager.Instance.AddObject(controller);
	}
#endregion

#region Protected Functions
#endregion

}
