using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

#region Fields
	[SerializeField] protected uint 			id;
	protected Dictionary<uint, ObjectState> 		unitStates;
#endregion

#region Override MonoBehaviour
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
#endregion
}
