using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Phalanx formation.
/// struct to hold the formation information
/// </summary>
public class PhalanxFormation {

	#region Fields
	List<Vector3> m_positions;
	
	#endregion
	
	#region Constructors
	public PhalanxFormation()
	{
		m_positions = new List<Vector3>();
	}
	#endregion
	
	
	#region Public API
	public void Add(Vector3 position)
	{
		m_positions.Add(position);
	}
	
	public void Remove()
	{
	}
	#endregion
}
