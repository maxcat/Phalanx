using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Latency
{
#region Fields
	[SerializeField] protected int 						minLatency;
	[SerializeField] protected int 						maxLatency;
#endregion

#region Getter and Setter
	public float LatencyInSeconds
	{
		get
		{
			float result = Random.Range(minLatency, maxLatency + 1) / 1000f;
			return result;
		}
	}
#endregion
}

public class ClientService : MonoBehaviour {

#region Fields
	// latency in ms
	[SerializeField] protected Latency 					latency;
	[SerializeField] protected uint 					clientID;
	[SerializeField] protected uint 					playerObjectID;

	[SerializeField] protected ServerSimulationService			serverSimuation;

	protected Dictionary<uint, ObjectClientController> 			objectPool;
#endregion

#region Getter and Setter
	public float LatencyInSeconds
	{
		get { return latency.LatencyInSeconds; }
	}

	public uint ClientID
	{
		get { return clientID; }
	}
#endregion

#region Override MonoBehaviour
	void Awake () { 
		init();
       	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable () {
	} 
 
	void OnDisable () {
	}
#endregion

#region Public API
	public void AddObject(uint id, ObjectClientController obj)
	{
		if(objectPool.ContainsKey(id))
		{
			Debug.LogWarning("[WARNING]ClientService->AddObject: id " + id + " already exist!");
			return;
		}

		if(obj == null)
		{
			Debug.LogWarning("[WARNING]ClientService->AddObject: GameObject can not be null! ");
			return;
		}

		objectPool.Add(id, obj);
	}

	public ObjectClientController GetObject(uint id)
	{
		if(!objectPool.ContainsKey(id))
		{
			Debug.LogWarning("[WARNING]ClientService->GetObject: GameObject with id " + id + " can not be found!");
			return null;
		}	

		return objectPool[id];
	}
#endregion

#region Event Listener
	public void OnReceiveStates(ObjectStatesData data)
	{
		this.StartCoroutine(receiveFlow(data));	
	}

	public void OnReceiveInput(Vector3 mousePosition)
	{
		if(objectPool.ContainsKey(playerObjectID))
		{
			objectPool[playerObjectID].OnReceiveInput(mousePosition);
		}
	}

	public void OnReceiveCommand(Command command)
	{
		this.StartCoroutine(postCommandFlow(command));
	}
#endregion

#region Protected Functions
	protected void init()
	{
		objectPool = new Dictionary<uint, ObjectClientController>();
	}

	protected void receiveStates(ObjectStatesData data)
	{
		foreach(uint objectID in data.Data.Keys)
		{
			ObjectClientController controller;
			ObjectState state = data.Data[objectID];
			if(!objectPool.ContainsKey(objectID))	
			{
				controller = ObjectClientController.CreateController(transform, objectID, state, serverSimuation.CommandDelayInState);
				objectPool.Add(objectID, controller);
				if(objectID == playerObjectID)
				{
					controller.OnObjectPostCommand += OnReceiveCommand;
				}
			}
			else
			{
				controller = objectPool[objectID];
				controller.OnUpdateState(state);
			}
		}
	}

	protected IEnumerator receiveFlow(ObjectStatesData data)
	{
		yield return new WaitForSeconds(LatencyInSeconds);
		receiveStates(data);
	}

	protected IEnumerator postCommandFlow(Command command)
	{
		yield return new WaitForSeconds(LatencyInSeconds); 
		serverSimuation.OnReceiveCommands(command);
	}
#endregion

}

