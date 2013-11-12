using UnityEngine;
using System.Collections;

public class GroundControl : MonoBehaviour {
	
	#region Fields
	private int m_lastGroundItemIndex;
	
	static readonly float BOUNDARY_Z = -10f;
	static readonly float ITEM_WIDTH = 10f;
	#endregion
	
	#region Mono
	// Use this for initialization
	void Start () {
		
		// reset the last ground item index
		m_lastGroundItemIndex = transform.GetChildCount() - 1;
		
	}
	
	// Update is called once per frame
	void Update () {
		
		// tan li, pending
		// use speed as speed per frame, might need to change to per sec
		
		// get the speed from the player data
		float speed = GlobalData.Shared().speed;
		Vector3 delta = new Vector3(0, 0, speed);
		
		// move the all ground item towards -z direction
		for(int i = 0; i < transform.childCount; i ++)
		{
			transform.FindChild(i.ToString()).localPosition -= delta;
		}
		
		// check for the position of the last ground item
		if(GetLastItem().transform.localPosition.z < BOUNDARY_Z)
		{
			// over the boundary, move the item to the top
			GetLastItem().transform.localPosition += new Vector3(0, 0, transform.GetChildCount() * ITEM_WIDTH);
			
			// update the last index
			m_lastGroundItemIndex = (m_lastGroundItemIndex + transform.GetChildCount() - 1) % transform.GetChildCount();
			
			// tan li, pending
			// update the last item's material
		}
	
	}
	
	
	#endregion
	
	#region Private
	GameObject GetLastItem()
	{
		return transform.FindChild(m_lastGroundItemIndex.ToString()).gameObject;
	}
	#endregion
}
