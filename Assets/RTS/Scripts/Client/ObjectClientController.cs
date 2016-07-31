using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectClientController : MonoBehaviour {

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
		states.Add(initState.StateTag, initState);
		this.currentTag = initState.StateTag;
		
		movementFlow = new ObjMovementFlow(gameObject, states, currentTag);
		movementFlow.Start(this);
	}
#endregion

#region Protected Functions
	protected void updateState()
	{
		
	}
#endregion

#region Event Handler
	public void OnUpdateState(ObjectState state)
	{
		if(states.ContainsKey(state.StateTag))
		{
			Debug.LogWarning("[WARNING]ObjectClientController->OnUpdateState: state tag " + state.StateTag + " already exist.");
		}
		else
		{
			states.Add(state.StateTag, state);
		}
	}
#endregion
}
