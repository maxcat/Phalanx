using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClientService : MonoBehaviour {

#region Fields
	// latency in ms
	[SerializeField] protected int 						latency = 100;
	[SerializeField] protected uint 					clientID;

	[SerializeField] protected ServerSimulationService			serverSimuation;
	[SerializeField] protected List<GameObject> 				prefabList;

	protected CircularTimeSteps 						timeSteps; 
	protected Dictionary<uint, ObjectClientController> 			objectPool;
#endregion

#region Getter and Setter
	public int Latency
	{
		get { return latency; }
	}

	public uint ClientID
	{
		get { return clientID; }
	}
#endregion

#region Override MonoBehaviour
	// Use this for initialization
	void Start () {
		init();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
#endregion

#region Public API
	public void PostCommand()
	{
		
	}

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
	public void OnReceiveTimeStep(TimeStep step)
	{
		timeSteps.Append(step);

		foreach(ObjectClientController controller in objectPool.Values)
		{
			controller.OnUpdateState(step.GetObjectState(controller.ObjectID));
		}
	}
#endregion

#region Protected Functions
	protected void init()
	{
		timeSteps = new CircularTimeSteps();
		objectPool = new Dictionary<uint, ObjectClientController>();
	}
#endregion

}

