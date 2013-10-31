using UnityEngine;
using System.Collections;

public class UnitControl : MonoBehaviour {
	
	
	#region Mono
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	// on collision enter
	void OnCollisionEnter(Collision collision)
	{
		Debug.Log("++++++enter collision with " + collision.gameObject.name);
	}
	
	void OnTriggerEnter(Collider other)
	{
		Debug.Log("+++++on " + gameObject.name + " trigger enter++++" + other.transform.parent.gameObject.name);
	}
	
	
	#endregion
}
