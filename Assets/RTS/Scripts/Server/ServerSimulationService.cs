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

#region Getter and Setter
	public uint CommandDelayInStep
	{
		get { return commandDelayInStep; }
	}
#endregion

#region Override MonoBehaviour
	// Use this for initialization
	IEnumerator Start () {

		serverTag = 1;
		// test code.
		ObjectManager.Instance.TestInit(serverTag);

		while(true)
		{
			uint startCommandTag = CommandManager.Instance.MoveReceivedCommandToPool();

			// initial states should cover commandDelayInStep * TimeStep.STATE_PER_TIME_STEP states.
			if(serverTag - commandDelayInStep >= 1)
			{
				uint commandTag = (serverTag - commandDelayInStep - 1) * (uint)TimeStep.STATES_PER_TIME_STEP + 1;
				if(startCommandTag > commandTag || startCommandTag == 0)
					startCommandTag = commandTag;
				else if(commandTag - startCommandTag > maxCommandStepDelay * (uint)TimeStep.STATES_PER_TIME_STEP)
					startCommandTag = commandTag - maxCommandStepDelay * (uint)TimeStep.STATES_PER_TIME_STEP;

				for(uint i = startCommandTag; i <= commandTag; i += (uint)TimeStep.STATES_PER_TIME_STEP)
				{
					ObjectManager.Instance.UpdateState(i, commandDelayInStep);
				}
			}

			// send the latest state to the client
			TimeStep step = ObjectManager.Instance.GenerateTimeStep(serverTag);

			for(int i = 0; i < clientList.Count; i ++)
			{
				clientList[i].OnReceiveTimeStep(step.Deserialize());
			}

			yield return new WaitForSeconds(TimeStep.STATES_PER_TIME_STEP * TimeStep.STATE_DURATION);
			serverTag ++;
		}
	}

	// Update is called once per frame
	void Update () {

	}
#endregion

#region Event Listener
	public void OnReceiveCommands(Command command)
	{
		Debug.LogError("===receive command " + command.SendTag + " owner " + command.OwnerID);
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
