using UnityEngine;
using System.Collections;

public class UnitControl : MonoBehaviour {
	
	
	#region Field
	UnitData				m_data;
	Phalanx					m_phalanx;
	
	public float 			m_colStrength;
	public float			m_colMass;
	
	float					m_acceleration;
	#endregion
	
	
	#region Mono
	// Use this for initialization
	void Start () {
		// get the parent phalanx
		m_phalanx = transform.parent.GetComponent<Phalanx>();
	}
	
	// Update is called once per frame
	void Update () {
		if(m_acceleration == 0)
			return;
		
		
	}
	
	
	void Awake () {
		// get the unit data
		m_data = GetComponent<UnitData>();
		
		// init the col strength and mass
		m_colMass = m_data.mass;
		m_colStrength = m_data.strength;
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
		// get the unit data
		UnitData collisionData = opponent.GetComponent<UnitData>();
		// this unit is the head of the column
		// only handle the player unit
		if(m_phalanx.isPlayerPhalanx)
		{
			// only move the player unit
			// calculate the player unit position
			transform.localPosition =  new Vector3(opponent.transform.localPosition.x,
				opponent.transform.localPosition.y,
				opponent.transform.localPosition.z - 
				opponent.GetComponent<UnitData>().length * AppConstant.UNIT_SIZE / 2);
		}
		
	}
	
	void OnCollideWithFriend(GameObject friend)
	{
		// get the unit data
		UnitData collisionData = friend.GetComponent<UnitData>();
		Debug.Log("++++++" + this.name + " enter collision with friend" + friend.name);
		// player unit collide with player unit
		if(m_data.rowIndex < collisionData.rowIndex)
		{
			// only handle the collision with the unit behind
			if(m_phalanx.isPlayerPhalanx)
			{
				// player collide with player
				CollidedUnits.shared().collidedWithPlayer(collisionData.colIndex, friend);
			}
			else
			{
				// enemy unit collide with the enemy unit
				CollidedUnits.shared().collidedWithEnemy(collisionData.colIndex, friend);
			}
		}
		
	}
	
	#endregion
	
	#region Public API
	public bool isPlayerUnit()
	{
		return m_phalanx.isPlayerPhalanx;
	}
	#endregion
}
