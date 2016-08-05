using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectClientController : MonoBehaviour {

#region Static Functions
	public static ObjectClientController CreateController(Transform parent, uint objectID, ObjectState initState, uint commandDelayInState)
	{
		GameObject prefab = Resources.Load("Unit" + objectID) as GameObject;
		GameObject instance = GameObject.Instantiate(prefab) as GameObject;
		instance.transform.SetParent(parent);	

		ObjectClientController controller = instance.GetComponent<ObjectClientController>();
		controller.Init(objectID, initState, commandDelayInState);

		return controller;
	}
#endregion

#region Events
	public delegate void ObjectPostCommandDelegate(Command command);
	public event ObjectPostCommandDelegate OnObjectPostCommand; 
#endregion

#region Fields
	[SerializeField] protected uint 			objectID;
	[SerializeField] protected uint 			currentTag;
	protected Dictionary<uint, ObjectState> 		states;

	protected ObjectFlow					mainFlow;
	protected Dictionary<uint, List<Command>> 		commands;	
	protected uint 						commandDelayInState;
#endregion

#region Getter and Setter
	public uint ObjectID
	{
		get { return objectID; }
	}
#endregion

#region Override MonoBehaviour
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
#endregion

#region Public API
	public void Init(uint objectID, ObjectState initState, uint commandDelayInState)
	{
		this.objectID = objectID;
		this.states = new Dictionary<uint, ObjectState>();
		this.commands = new Dictionary<uint, List<Command>>();
		this.currentTag = initState.StateTag;
		this.commandDelayInState = commandDelayInState;

		OnUpdateState(initState);

		startObjectFlow();
	}
#endregion

#region Protected Functions
	protected void startObjectFlow()
	{
		mainFlow = new ObjectFlow(gameObject, states);
		mainFlow.Start(this);
	}

	public ObjectState predictState(uint stateTag)
	{
		if(states.ContainsKey(stateTag))
		{
			uint nextTag = stateTag + 1;

			List<Command> commandList = null;

			uint commandTag = nextTag - commandDelayInState;
			if(commands.ContainsKey(commandTag))
			{
				commandList = commands[commandTag];
			}

			ObjectState nextState = states[stateTag].GenerateNextState(commandList);
			nextState.IsPrediction = true;
			Debug.Log("[INFO]ObjectClientController->PredictState: predict state for tag " + nextTag);
			states.Add(nextTag, nextState);
			return nextState;
		}
		return null;
	}
#endregion

#region Event Handler
	public void OnUpdateState(ObjectState state)
	{
		if(this.states.ContainsKey(state.StateTag))
		{
			if(!state.IsPrediction)
			{
				Debug.Log("[INFO]ObjectClientController->OnUpdateState: override the predicted state with the state from server with tag " + state.StateTag + " for object " + objectID + " on client " + transform.parent.gameObject.name);
				states[state.StateTag] = state;
			}
		}	
		else
		{
			states.Add(state.StateTag, state);
		}
	}


	public void OnReceiveInput(Vector3 mousePosition)
	{
		uint commandTag = currentTag;
		if(!commands.ContainsKey(commandTag))
		{
			Vector2 destPos = (Vector2)transform.parent.InverseTransformPoint(mousePosition);	
			MoveToPosCommand command = new MoveToPosCommand(commandTag, objectID, destPos);

			List<Command> commandList = new List<Command>();
			commandList.Add(command);
			commands.Add(commandTag, commandList);

			OnObjectPostCommand(command);
		}
	}

	public ObjectState GetNextState()
	{
		ObjectState state;
		if(states.ContainsKey(currentTag))
		{
			state = states[currentTag];
		}
		else if(states.ContainsKey(currentTag - 1))
		{
			uint previousStateTag = currentTag - 1;
			state = predictState(previousStateTag); 
		}
		else
		{
			Debug.LogError("[ERROR]ObjectClientController->GetNextState: can not find state for both tag " + currentTag + " and previous tag " + (currentTag - 1));
			return null;
		}
		currentTag ++;
		return state;
	}
#endregion
}
