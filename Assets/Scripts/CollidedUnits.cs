using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollidedCol
{
	public float 						m_mass;
	public float						m_force; // force in direction of z, player advancing direction
	public List<GameObject>				m_units;
	public float						m_speed;
	public string						m_name;
	
	public CollidedCol()
	{
		// defautl constructor
		m_mass = 0;
		m_force = 0;
		m_speed = 0;
		m_units = new List<GameObject>();
	}
	
	public void add(GameObject unit)
	{
		m_mass += unit.GetComponent<UnitData>().mass;
		m_units.Add(unit);
	}
	
	public void addPlayerUnit(GameObject player)
	{
		// add to the tail
		m_mass += player.GetComponent<UnitData>().mass;
		m_force += player.GetComponent<UnitData>().strength;
		m_units.Add(player);
	}
	
	public void addEnemyUnit(GameObject enemy)
	{
		// add to the head
		m_mass += enemy.GetComponent<UnitData>().mass;
		m_force -= enemy.GetComponent<UnitData>().strength;
		m_units.Insert(0, enemy);
	}
	
	public void updateUnitState(float acceleration, float speed)
	{
		m_speed = speed;
		foreach(GameObject unit in m_units)
		{
			unit.GetComponent<UnitControl>().updateUnitState(acceleration, speed);
		}
		
		//Debug.Log("++++update collided units state, speed is " + speed + " acceleration is " + acceleration);
	}
	
	public bool isInTheSameCollider(GameObject obj1, GameObject obj2)
	{
		return m_units.Contains(obj1) && m_units.Contains(obj2);
	}
	
	public void removeUnit(GameObject unit)
	{
		if(m_units.Contains(unit))
		{
			// tan li, pending
			// need to divide collide list to sub lists
			// need to implement in collideUnits instead of here
			m_mass -= unit.GetComponent<UnitData>().mass;
			m_units.Remove(unit);
		}
		else
		{
			Debug.LogWarning("[CollideUnit::removeUnit] unit " + unit.name + " is not in the collided list");
		}
	}
}

public class CollidedUnits {
	
	// collision units list per column
	// tan li, pending
	// for a single col, can has more than one collided column
	// occur when one unit in the col is killed
	Dictionary<int, CollidedCol> m_collidedDic;
	
	static CollidedUnits m_instance;
	
	#region Mono
	// Awake
	void Awake () {
		
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#endregion
	
	#region Constructor
	public CollidedUnits()
	{
		// init the dicionary
		m_collidedDic = new Dictionary<int, CollidedCol>();
		
		for(int i = 0; i < AppConstant.MAX_COL; i ++)
		{
			m_collidedDic[i] = new CollidedCol();
		}
	}
	
	#endregion
	
	#region Public API
	
	public static CollidedUnits shared()
	{
		// singleton lazy creationg
		if(m_instance == null)
		{
			m_instance = new CollidedUnits();
		}
		return m_instance;
	}
	
	public float getColMass(int col)
	{
		return m_collidedDic[col].m_mass;
	}
	
	public float getColForce(int col)
	{
		return m_collidedDic[col].m_force;
	}
	
	public bool isInSameColliderCol(int col, GameObject obj1, GameObject obj2)
	{
		return m_collidedDic[col].isInTheSameCollider(obj1, obj2);
	}
	
	public void init(int col, GameObject player)
	{
		// calcluate the init speed
		float initSpeed = calculateInitialSpeed(m_collidedDic[col].m_mass, 
			m_collidedDic[col].m_speed, 
			player.GetComponent<UnitData>().mass, 
			player.GetComponent<UnitControl>().m_speed);
		
		m_collidedDic[col].addPlayerUnit(player);
		
		m_collidedDic[col].updateUnitState(0, initSpeed);
	}
	
	
	public void collidedWithPlayer(int col, GameObject player)
	{
		// calculate the init speed
		float initSpeed = calculateInitialSpeed(m_collidedDic[col].m_mass, 
			m_collidedDic[col].m_speed, 
			player.GetComponent<UnitData>().mass, 
			player.GetComponent<UnitControl>().m_speed);
			
		m_collidedDic[col].addPlayerUnit(player);
		
		onCollision(col, initSpeed);
	}
	
	public void collidedWithEnemy(int col, GameObject enemy)
	{
		// calcluate the init speed
		float initSpeed = calculateInitialSpeed(m_collidedDic[col].m_mass, 
			m_collidedDic[col].m_speed, 
			enemy.GetComponent<UnitData>().mass, 
			enemy.GetComponent<UnitControl>().m_speed);	
		
		m_collidedDic[col].addEnemyUnit(enemy);
		
		onCollision(col, initSpeed);
	}
	
	#endregion
	
	#region Call back functions

	#endregion
	
	#region Internal
	float calculateAcceleration(float mass, float force)
	{
		return force / (mass + 10000);
	}
	
	float calculateInitialSpeed(float mass1, float speed1, float mass2, float speed2)
	{
		//Debug.Log("+++mass 1 is " + mass1 + " mass 2 is " + mass2 + " speed 1 is " + speed1 + " speed 2 is " + speed2);
		return (mass1 * speed1 + mass2 * speed2) / (mass1 + mass2);
	}
	
	
	void onCollision(int col, float speed)
	{
		// calculate acceleration
		float acceleration = calculateAcceleration(m_collidedDic[col].m_mass, 
			m_collidedDic[col].m_force);
		m_collidedDic[col].updateUnitState(acceleration, speed);
	}
	
	#endregion
}
