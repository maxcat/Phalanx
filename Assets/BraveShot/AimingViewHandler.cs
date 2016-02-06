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
	public void drawDirectionLine(Vector3 input)
	{
		LineRenderer lineRenderer = GetComponent<LineRenderer>();

		lineRenderer.useWorldSpace = false;
		lineRenderer.SetPosition(0, Vector3.zero);
		lineRenderer.SetPosition(1, new Vector3(input.x, 0f, input.y));
	}
	#endregion
}
