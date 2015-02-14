using UnityEngine;
using System.Collections;

public class Service : MonoBehaviour {



	#region Override MonoBehaviour
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	#endregion

	#region Static Fields
	static GameObject m_serviceObject;
	#endregion
	
	#region Static Public API
	public static T get<T>() where T : Singleton
	{
		if(m_serviceObject == null)
		{
			m_serviceObject = new GameObject();
			DontDestroyOnLoad(m_serviceObject);
			m_serviceObject.name = "BackgroundService";
		}

		T service = m_serviceObject.GetComponent<T>();
		if(service == null)
		{
			service = m_serviceObject.AddComponent<T>();
		}

		return service;
	}

	public static void remove<T>() where T : Singleton
	{
		if(m_serviceObject == null)
			return;

		T service = m_serviceObject.GetComponent<T>();
		if(service != null)
			GameObject.Destroy(service);
	}
	#endregion
}
