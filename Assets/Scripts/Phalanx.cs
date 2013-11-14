using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Phalanx : MonoBehaviour {
	
	// test source tree
	#region Fields	
	// list of columns
	// store the unit in columns
	List<List<GameObject>>		m_UnitMatrix;
	
	
	float 						m_phalanxWidth;
	float						m_phalanxHeight;
	
	int 						m_unitCount;
	
	bool						m_isPlayerPhalanx;
	
	// debug purpose
	Vector3						m_LTVertex;
	Vector3						m_RTVertex;
	Vector3						m_LBVertex;
	Vector3						m_RBVertex;
	Color						m_color;
	
	public bool isPlayerPhalanx {
		get
		{
			return this.m_isPlayerPhalanx;
		}
		set
		{
			m_isPlayerPhalanx = value;
		}
	}
		
	
	#endregion
	
	
	#region Mono
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// draw the phalanx for debug 
		m_LTVertex = new Vector3(transform.localPosition.x - m_phalanxWidth / 2, 
			0, 
			transform.localPosition.z + m_phalanxHeight / 2);
		m_RTVertex = new Vector3(transform.localPosition.x + m_phalanxWidth / 2, 
			0, 
			transform.localPosition.z + m_phalanxHeight / 2);
		m_LBVertex = new Vector3(transform.localPosition.x - m_phalanxWidth / 2, 
			0, 
			transform.localPosition.z - m_phalanxHeight / 2);
		m_RBVertex = new Vector3(transform.localPosition.x + m_phalanxWidth / 2,
			0,
			transform.localPosition.z - m_phalanxHeight / 2);
		
		if(isPlayerPhalanx)
		{
			m_color = Color.red;
			Debug.DrawLine(m_LTVertex, m_RTVertex, m_color);
			Debug.DrawLine(m_RTVertex, m_RBVertex, m_color);
			Debug.DrawLine(m_RBVertex, m_LBVertex, m_color);
			Debug.DrawLine(m_LBVertex, m_LTVertex, m_color);
		}
		else
			m_color = Color.blue;
		
		
		
		
	
	}
	
	void Awake() {
		// init the matrix
		InitUnitMatirx();
		
		// calculate the width of the height of the phalanx
		m_phalanxWidth = AppConstant.MAX_COL * AppConstant.UNIT_SIZE + 
			(AppConstant.MAX_COL - 1) * AppConstant.UNIT_INTERVAL;		
		m_phalanxHeight = AppConstant.MAX_ROW * AppConstant.UNIT_SIZE + 
			(AppConstant.MAX_ROW - 1) * AppConstant.UNIT_INTERVAL;
		
		
		
		
		
	}
	
	
	#endregion
	
	#region Public API
	
	public GameObject getUnit(int row, int col)
	{
		return m_UnitMatrix[col][row];
	}
	//public void AddUnit(string prefabName, Vector3 position, Vector3 rotation, Vector3 scale)
	public void AddUnit(string prefabName, bool isPlayerUnit, Vector3 scale, int rowIndex, int colIndex, float abSpeed)
	{
		//GameObject unit = Instantiate(Resources.Load(prefabName), position, Quaternion.identity) as GameObject;
		GameObject unit = Instantiate(Resources.Load(prefabName)) as GameObject;
		
		// add as child
		unit.transform.parent = this.transform;
		
		// update the position, rotation and scale
		unit.transform.localScale = scale;
		
		
		// set the facing direction of the unit
		Quaternion rotationQ = new Quaternion();
		if(isPlayerUnit)
			rotationQ.SetLookRotation(Vector3.forward);
		else
			rotationQ.SetLookRotation(Vector3.back);
		unit.transform.localRotation = rotationQ;
		
		
		// tan li, pending
		// need to implement according to the size and the shape of the unit
		unit.transform.localPosition = CalculateUnitPosition(rowIndex, colIndex, isPlayerUnit);
		
		// update the matrix
		m_UnitMatrix[colIndex][rowIndex] = unit;
		
		// update the unit data
		unit.GetComponent<UnitData>().colIndex = colIndex;
		unit.GetComponent<UnitData>().rowIndex = rowIndex;
		
		// update the absolute speed of the unit
		unit.GetComponent<UnitControl>().m_speed = abSpeed;
		
		// update count
		m_unitCount ++;
		
	}
	
	//public void PlayAnimation(int index, string animationName)
	public void PlayAnimation(int rowIndex, int colIndex, string animationName)
	{
		Animation animation = m_UnitMatrix[colIndex][rowIndex].GetComponent<Animation>();
		animation.wrapMode = WrapMode.Loop;
		animation.Play(animationName);
	}
	
	#endregion
	
	
	#region Internal
	
	private Vector3 CalculateUnitPosition(int row, int col, bool isPlayerUnit)
	{
		float startX = - (m_phalanxWidth / 2 - AppConstant.UNIT_SIZE / 2);
		float startZ;
		Vector3 result;
		if(isPlayerUnit)
		{
			startZ = m_phalanxHeight / 2 - AppConstant.UNIT_SIZE /  2;
			result = new Vector3(startX + col * (AppConstant.UNIT_SIZE + AppConstant.UNIT_INTERVAL),
				0,
				startZ - row * (AppConstant.UNIT_SIZE + AppConstant.UNIT_INTERVAL));
		}
		else
		{
			startZ = - m_phalanxHeight / 2 + AppConstant.UNIT_SIZE /  2;
			result = new Vector3(startX + col * (AppConstant.UNIT_SIZE + AppConstant.UNIT_INTERVAL),
				0,
				startZ + row * (AppConstant.UNIT_SIZE + AppConstant.UNIT_INTERVAL));
		}
		
		
		
		
		return result;
	}
	
	private void InitUnitMatirx()
	{
		// init the unit count
		m_unitCount = 0;
		
		// init the matrix
		m_UnitMatrix = new List<List<GameObject>>();
		for(int col = 0; col < AppConstant.MAX_COL; col ++)
		{
			List<GameObject> colList = new List<GameObject>();
			for(int row= 0; row < AppConstant.MAX_ROW; row ++)
			{
				colList.Add(null);
			}
			m_UnitMatrix.Add(colList);
		}
	}
	#endregion
	
	
	
}
