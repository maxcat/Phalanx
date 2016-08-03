﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectClientController : MonoBehaviour {

#region Static Functions
	public static ObjectClientController CreateController(Transform parent, uint objectID, ObjectState initState, uint commandDelayInStep)
	{
		GameObject prefab = Resources.Load("Unit" + objectID) as GameObject;
		GameObject instance = GameObject.Instantiate(prefab) as GameObject;
		instance.transform.SetParent(parent);	

		ObjectClientController controller = instance.GetComponent<ObjectClientController>();
		controller.Init(objectID, initState, commandDelayInStep);

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
	protected uint 						commandDelayInStep;
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
	public void Init(uint objectID, ObjectState initState, uint commandDelayInStep)
	{
		this.objectID = objectID;
		this.states = new Dictionary<uint, ObjectState>();
		this.commands = new Dictionary<uint, List<Command>>();
		this.currentTag = initState.StateTag;
		this.commandDelayInStep = commandDelayInStep;

		OnUpdateState(initState);

		startObjectFlow();
	}
#endregion

#region Protected Functions
	protected void startObjectFlow()
	{
		mainFlow = new ObjectFlow(gameObject, states, currentTag);
		mainFlow.Start(this);
	}
#endregion

#region Event Handler
	public void OnUpdateState(ObjectState state)
	{
		if(states.ContainsKey(state.StateTag))
		{
			if(!state.IsPrediction)
			{
				Debug.Log("[INFO]ObjectClientController->OnUpdateState: override the predicted state with the state from server with tag " + state.StateTag);
				states[state.StateTag] = state;
			}
		}
		else
		{
			states.Add(state.StateTag, state);
			currentTag = state.StateTag;
		}
	}

	public ObjectState PredictState(uint stateTag)
	{
		if(states.ContainsKey(stateTag))
		{
			uint nextTag = stateTag + 1;

			List<Command> commandList = null;

			uint commandTag = nextTag - commandDelayInStep;
			if(commands.ContainsKey(commandTag))
			{
				commandList = commands[commandTag];
			}

			ObjectState nextState = states[stateTag].GenerateNextState(commandList);
			nextState.IsPrediction = true;
			Debug.Log("[INFO]ObjectClientController->PredictState: predict state for tag " + nextTag);
			states.Add(nextTag, nextState);
			currentTag = nextTag;
			return nextState;
		}
		return null;
	}

	public void OnReceiveInput(Vector3 mousePosition)
	{
		if(!commands.ContainsKey(currentTag))
		{
			Vector2 destPos = (Vector2)transform.parent.InverseTransformPoint(mousePosition);	
			MoveToPosCommand command = new MoveToPosCommand(currentTag, objectID, destPos);

			List<Command> commandList = new List<Command>();
			commandList.Add(command);
			commands.Add(currentTag, commandList);

			OnObjectPostCommand(command);
		}
	}
#endregion
}
