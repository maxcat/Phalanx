using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerSimulationService : MonoBehaviour {

#region Fields
	[SerializeField] protected List<ClientService> 			clientList;
	[SerializeField] protected uint 				commandDelayInState = 1;
	[SerializeField] protected uint 				maxCommandStateDelay = 3;

	protected uint 							serverTag;
#endregion

#region Getter and Setter
	public uint CommandDelayInState
	{
		get { return commandDelayInState; }
	}
#endregion

#region Override MonoBehaviour
	// Use this for initialization
	IEnumerator Start () {

		serverTag = 1;
		// test code.
		ObjectManager.Instance.TestInit(serverTag);
		float timeElapse = 0f;

		while(true)
		{
			uint startCommandTag = CommandManager.Instance.MoveReceivedCommandToPool();

			if(serverTag > commandDelayInState)
			{
				uint startTag = startCommandTag + commandDelayInState;
				if(startTag > serverTag || startCommandTag == 0)
					startTag = serverTag;
				else if(serverTag - startTag > maxCommandStateDelay)
					startTag = serverTag - maxCommandStateDelay;

				for(uint i = startTag; i < serverTag; i ++)
				{
					ObjectManager.Instance.UpdateState(i, commandDelayInState);
				}
			}

			ObjectManager.Instance.UpdateState(serverTag, commandDelayInState);

			// send the latest state to the client
			ObjectStatesData stateData = ObjectManager.Instance.GenerateStateData(serverTag);

			for(int i = 0; i < clientList.Count; i ++)
			{
				clientList[i].OnReceiveStates(stateData);
			}

			while(timeElapse < ObjectState.DURATION)
			{
				float deltaTime = Time.deltaTime;
				yield return null;
				timeElapse += deltaTime;
			}
			timeElapse -= ObjectState.DURATION;
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
