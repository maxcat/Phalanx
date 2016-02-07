using UnityEngine;
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

	#region Public API
	public virtual void init()
	{
		m_body = GetComponent<Rigidbody>();
	}
	public virtual void releaseBall(Vector3 input)
	{
		m_body.AddForce(new Vector3(input.x, 0f, input.y) * m_multiply + transform.position, m_releaseForceMode);
	}
	#endregion
}
