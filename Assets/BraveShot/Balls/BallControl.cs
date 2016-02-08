﻿using UnityEngine;
using System.Collections;

public class BallControl : MonoBehaviour {

	#region Fields
	[SerializeField] protected ForceMode		m_releaseForceMode;
	[SerializeField] float						m_multiply = 1f;
	protected Rigidbody							m_body;
	protected SphereCollider					m_collider;
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
//		print("No longer in contact with " + collisionInfo.transform.name);
	}

	void OnTriggerEnter(Collider other) {
	}

	void OnTriggerExit(Collider other) {
	}

	void OnTriggerStay(Collider other) {
	}
	#endregion

	#region Public API
	public virtual void init()
	{
		m_body = GetComponent<Rigidbody>();
	}
	public virtual void releaseBall(Vector3 input)
	{
		m_body.AddForce(new Vector3(input.x, 0f, input.y).normalized * m_multiply + transform.position, m_releaseForceMode);

		m_body.AddTorque(transform.up * 100f, m_releaseForceMode);
	}
	#endregion
}
