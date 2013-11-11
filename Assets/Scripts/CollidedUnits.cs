using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollidedCol
{
	public float 						m_mass;
	public List<GameObject>				m_units;
	
	public CollidedCol()
	{
		// defautl constructor
		m_mass = 0;
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
		m_units.Add(player);
	}
	
	public void addEnemyUnit(GameObject enemy)
	{
		// add to the head
		m_mass += enemy.GetComponent<UnitData>().mass;
		m_units.Insert(0, enemy);
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

public class CollidedUnits : MonoBehaviour {
	
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
	
	public void collidedWithPlayer(int col, GameObject player)
	{
		m_collidedDic[col].addPlayerUnit(player);
	}
	
	public void collidedWithEnemy(int col, GameObject enemy)
	{
		m_collidedDic[col].addEnemyUnit(enemy);
	}
	
	#endregion
	
	#region Call back functions

	#endregion
}
