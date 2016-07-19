using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerSimulationService : MonoBehaviour {

#region Fields
	[SerializeField] protected List<ClientService> 			clientList;
#endregion

#region Override MonoBehaviour
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
#endregion

#region Server Flow
	protected IEnumerator mainEnumerator()
	{
		yield return null;	
	}
#endregion

}
