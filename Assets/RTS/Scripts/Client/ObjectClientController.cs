using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectClientController : MonoBehaviour {

#region Static Functions
	public static ObjectClientController CreateController(Transform parent, uint objectID, ObjectState initState)
	{
		GameObject prefab = Resources.Load("Unit" + objectID) as GameObject;
		GameObject instance = GameObject.Instantiate(prefab) as GameObject;
		instance.transform.SetParent(parent);	

		ObjectClientController controller = instance.GetComponent<ObjectClientController>();
		controller.Init(objectID, initState);

		return controller;
	}
#endregion

#region Fields
	[SerializeField] protected uint 			objectID;
	[SerializeField] protected uint 			currentTag;
	protected Dictionary<uint, ObjectState> 		states;

	protected ObjMovementFlow				movementFlow;
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
	public void Init(uint objectID, ObjectState initState)
	{
		this.objectID = objectID;
		states = new Dictionary<uint, ObjectState>();
		this.currentTag = initState.StateTag;

		OnUpdateState(initState);

		startObjectFlow();
	}
#endregion

#region Protected Functions
	protected void startObjectFlow()
	{
		movementFlow = new ObjMovementFlow(gameObject, states, currentTag);
		movementFlow.Start(this);
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
			ObjectState nextState = states[stateTag].GenerateNextState(null);
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
		Debug.LogWarning("=====input mouse position is " + mousePosition + " at tag " + currentTag);		

	}
#endregion
}
