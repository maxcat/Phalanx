using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Phalanx : MonoBehaviour {
	
	// test source tree
	#region Fields
	List<GameObject>			m_UnitList;
	
	public List<GameObject>		unitList 
	{
		get
		{
			// lazy creation 
			if(m_UnitList == null)
				m_UnitList = new List<GameObject>();
			return m_UnitList;
		}
	}
	#endregion
	
	
	#region Mono
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Awake() {
		m_UnitList = new List<GameObject>();
	}
	
	#endregion
	
	#region Public API
	public void AddUnit(string prefabName, Vector3 position, Vector3 rotation, Vector3 scale)
	{
		//GameObject unit = Instantiate(Resources.Load(prefabName), position, Quaternion.identity) as GameObject;
		GameObject unit = Instantiate(Resources.Load(prefabName)) as GameObject;
		
		// add as child
		unit.transform.parent = this.transform;
		
		// update the position, rotation and scale
		unit.transform.localScale = scale;
		unit.transform.localPosition = position;
		
		Quaternion rotationQ = new Quaternion();
		rotationQ.SetLookRotation(rotation);
		unit.transform.localRotation = rotationQ;
		
		// add to the list
		this.unitList.Add(unit);
	}
	
	public void PlayAnimation(int index, string animationName)
	{
		Animation animation = m_UnitList[index].GetComponent<Animation>();
		animation.wrapMode = WrapMode.Loop;
		animation.Play(animationName);
	}
	
	#endregion
	
	
	
}
