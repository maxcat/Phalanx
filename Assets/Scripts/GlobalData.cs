using UnityEngine;
using System.Collections;

public class GlobalData {

	static GlobalData m_instance;
	public float m_speed;
	
	#region Constructor
	public GlobalData()
	{
		m_speed = 0;
	}
	#endregion
	
	#region Accessor
	public static GlobalData Shared()
	{
		// lazy creation
		if(m_instance == null)
		{
			m_instance = new GlobalData();
		}
		return m_instance;
	}
	
	public float speed
	{
		get 
		{
			return m_speed;
		}
		set
		{
			m_speed = value;
		}
	}
	#endregion
}
