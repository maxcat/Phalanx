using UnityEngine;
using System.Collections;

public class UnitControl : MonoBehaviour {
	
	
	#region Field
	UnitData				m_data;
	Phalanx					m_phalanx;
	#endregion
	
	
	#region Mono
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// get the unit data
		m_data = GetComponent<UnitData>();
		
		// get the parent phalanx
		m_phalanx = transform.parent.GetComponent<Phalanx>();
	
	}
	
	
	// on collision enter
	void OnCollisionEnter(Collision collisionInfo)
	{
		// filter the ground plane
		GameObject collisionObj = collisionInfo.gameObject;
		if(collisionObj.layer == AppConstant.GROUND_LAYER)
		{
			return;
		} 
		
		// get the unit data
		UnitData collisionData = collisionObj.GetComponent<UnitData>();
		
		if(m_phalanx.isPlayerPhalanx == 
			collisionObj.transform.parent.GetComponent<Phalanx>().isPlayerPhalanx)
		{
			OnCollideWithFriend(collisionObj);
		}
		else
		{
			OnCollideWithOpponent(collisionObj);
		}
		
		
	}
	
	void OnCollisionStay(Collision collisionInfo)
	{
		
	}
	
	void OnCollisionExit(Collision collisionInfo)
	{
	}
	
	
	#endregion
	
	#region Internal
	void OnCollideWithOpponent(GameObject opponent)
	{
		Debug.Log("++++++" + this.name + " enter collision with opponent" + opponent.name);
	}
	
	void OnCollideWithFriend(GameObject friend)
	{
		Debug.Log("++++++" + this.name + " enter collision with friend" + friend.name);
	}
	
	#endregion
}
