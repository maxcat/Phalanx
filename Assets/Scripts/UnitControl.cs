using UnityEngine;
using System.Collections;

public class UnitControl : MonoBehaviour {
	
	
	#region Field
	UnitData				m_data;
	Phalanx					m_phalanx;
	public float 			m_speed;	// absolute speed of the unit
	public float 			m_acceleration;
	#endregion
	
	
	#region Mono
	// Use this for initialization
	void Start () {
		// get the parent phalanx
		m_phalanx = transform.parent.GetComponent<Phalanx>();
	}
	
	// Update is called once per frame
	void Update () {
		// get the phalanx speed
		float phalanxSpeed = GlobalData.Shared().speed;
		// calculate the relative speed
		float relativeSpeed;
		
		// set the up limit of the speed
		if(m_speed >= phalanxSpeed)
		{
			m_acceleration = 0;
			m_speed = phalanxSpeed;
		}
		
		if(isPlayerUnit())
		{
			relativeSpeed = m_speed - phalanxSpeed;
			updatePlayerUnitPosition();
			Debug.Log ("+++player  speed is " + relativeSpeed + " " + m_acceleration);
			
		}
		else
		{
			relativeSpeed = -m_speed;
			updateEnemyUnitPosition();
			Debug.Log ("+++enemy  speed is " + relativeSpeed + " " + m_acceleration);
		}
		
		
		
		
		// update position
		transform.Translate(new Vector3(0, 0, relativeSpeed));
		
		// update speed
		m_speed += m_acceleration;
	}
	
	
	void Awake () {
		// get the unit data
		m_data = GetComponent<UnitData>();
	}
	
	
	// on collision enter
	void OnCollisionEnter(Collision collisionInfo)
	{
		// filter the ground plane
		// or inside the same collider
		GameObject collisionObj = collisionInfo.gameObject;
		if(collisionObj.layer == AppConstant.GROUND_LAYER || 
			CollidedUnits.shared().isInSameColliderCol(collisionObj.GetComponent<UnitData>().colIndex, 
			gameObject, 
			collisionObj))
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
			/*
			transform.localPosition =  new Vector3(opponent.transform.localPosition.x,
				opponent.transform.localPosition.y,
				opponent.transform.localPosition.z - 
				opponent.GetComponent<UnitData>().length * AppConstant.UNIT_SIZE / 2);
				*/
			CollidedUnits.shared().collidedWithEnemy(collisionData.colIndex, opponent);
			
		}
		
	}
	
	void OnCollideWithFriend(GameObject friend)
	{
		// get the unit data
		UnitData collisionData = friend.GetComponent<UnitData>();
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
	
	void updatePlayerUnitPosition()
	{
		
		
	}
	
	
	void updateEnemyUnitPosition()
	{
	}
	
	#endregion
	
	#region Public API
	public bool isPlayerUnit()
	{
		return m_phalanx.isPlayerPhalanx;
	}
	
	public void updateUnitState(float acceleration, float speed)
	{
		m_speed = speed;
		m_acceleration = acceleration;
	}
	#endregion
}
