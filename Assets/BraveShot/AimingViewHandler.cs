using UnityEngine;
using System.Collections;

public class AimingViewHandler : MonoBehaviour {

	#region Override MonoBehaviour
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	#endregion

	#region Public API
	public virtual void drawDirectionLine(Vector3 input)
	{
		LineRenderer lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.enabled = true;

		lineRenderer.useWorldSpace = true;
		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, transform.position + new Vector3(input.x, 0f, input.y));
	}

	public virtual void removeLine()
	{
		LineRenderer lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.enabled = false;
	}
	#endregion
}
