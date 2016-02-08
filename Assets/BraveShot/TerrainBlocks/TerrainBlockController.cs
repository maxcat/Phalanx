using UnityEngine;
using System.Collections;

public class TerrainBlockController : MonoBehaviour {

	#region Override MonoBehaviour
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	#endregion

	#region Override Collision Event
	void OnCollisionEnter(Collision collision) {
		foreach (ContactPoint contact in collision.contacts) {
			Debug.DrawRay(contact.point, contact.normal * 10, Color.white);
		}
	}
	
	void OnCollisionStay(Collision collisionInfo) {
		foreach (ContactPoint contact in collisionInfo.contacts) {
			Debug.DrawRay(contact.point, contact.normal * 10, Color.white);
		}
	}
	
	void OnCollisionExit(Collision collisionInfo) {
		print("No longer in contact with " + collisionInfo.transform.name);
	}
	
	void OnTriggerEnter(Collider other) {

		other.rigidbody.AddForce(Vector3.forward * 100 + other.transform.position, ForceMode.Force);
	}
	
	void OnTriggerExit(Collider other) {
	}
	
	void OnTriggerStay(Collider other) {
		other.rigidbody.AddForce(Vector3.forward * 100 + other.transform.position, ForceMode.Force);
	}

	#endregion
}
