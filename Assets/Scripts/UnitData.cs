using UnityEngine;
using System.Collections;

public class UnitData : MonoBehaviour {
	
	#region Fields
	[SerializeField] float		m_mass;
	[SerializeField] float		m_strength;
	[SerializeField] int		m_width;
	[SerializeField] int 		m_length;
	
	[SerializeField] int		m_colIndex;
	[SerializeField] int		m_rowIndex;
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
			m_colIndex = value;
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
			m_rowIndex = value;
		}
	}
	
	public float mass
	{
		get
		{
			return m_mass;
		}
	}
	
	public float strength
	{
		get
		{
			return m_strength;
		}
	}
	
	public int width
	{
		get 
		{
			return m_width;
		}
	}
	public int length
	{
		get
		{
			return m_length;
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
