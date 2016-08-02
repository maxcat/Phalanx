﻿using UnityEngine;
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
		// test code.
		ObjectManager.Instance.TestInit(serverTag);

		while(true)
		{
			TimeStep step = new TimeStep(serverTag);

			uint startCommandTag = CommandManager.Instance.MoveReceivedCommandToPool();

			uint commandTag = serverTag - commandDelayInStep;
			Debug.LogWarning("====serve step " + serverTag + " with command " + commandTag + " start command tag is " + startCommandTag);
			if(commandTag >= 1)
			{
				if(startCommandTag > commandTag)
					startCommandTag = commandTag;
				else if(commandTag - startCommandTag > maxCommandStepDelay)
					startCommandTag = commandTag - maxCommandStepDelay;

				for(uint i = startCommandTag; i <= commandTag; i ++)
				{
					ObjectManager.Instance.UpdateState(i, commandDelayInStep);
				}
			}
			
			// send the latest state to the client
			step.ObjectStates = ObjectManager.Instance.GetStates(serverTag);

			for(int i = 0; i < clientList.Count; i ++)
			{
				clientList[i].OnReceiveTimeStep(step.Deserialize());
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
		CommandManager.Instance.OnReceiveCommand(command);
	}
#endregion

#region Public API
	public void AddObjectController(ObjectController controller)
	{
		ObjectManager.Instance.AddObject(controller);
	}
#endregion

}
