using UnityEngine;
using System.Collections;

public class UnitData : MonoBehaviour {
	
	#region Fields
	int					m_colIndex;
	int					m_rowIndex;
	#endregion
	
	#region Accessor
	public int colIndex
	{
		get
		{
			return m_colIndex;
		}
		set
		{
			m_rowIndex = value;
		}
			
	}
	
	public int rowIndex
	{
		get 
		{
			return m_rowIndex;
		}
		set
		{
			m_colIndex = value;
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
	#endregion
	
	
	#region Public API
	#endregion
}
