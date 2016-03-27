using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Service : MonoBehaviour {

	#region Override MonoBehaviour
	void Awake () {
		// init all the static service inside the prefab.
		List<Singleton> singletonList = getSingletonList();

		if(singletonList != null)
		{
			for(int i = 0; i < singletonList.Count; i ++)
			{
				singletonList[i].init();
			}
		}
	}

	void OnApplicationPause	()
	{
		List<Singleton> singletonList = getSingletonList();

		if(singletonList != null)
		{
			for(int i = 0; i < singletonList.Count; i ++)
			{
				singletonList[i].onGamePaused();
			}
		}
	}

	void Start () {
	
	}
	
	void Update () {
	
	}

	void OnLevelWasLoaded (int level) {

		List<Singleton> singletonList = getSingletonList();

		if(singletonList != null)
		{
			for(int i = 0; i < singletonList.Count; i ++)
			{
				singletonList[i].onSceneChanged(level);
			}
		}

		List<Singleton> removeList = getInPersistSingletonList();
		if(removeList != null)
		{
			for(int i = 0; i < removeList.Count; i ++)
			{
				removeList[i].preRemoved();
				GameObject.Destroy(removeList[i]);
			}
		}
	}
	#endregion

	#region Static Fields
	static GameObject m_serviceObject;
	#endregion

#region Protected Functions
	protected List<Singleton> getSingletonList()
	{
		List<Singleton> wholeList = new List<Singleton>(this.GetComponents<Singleton>());
		return wholeList;
	}

	protected List<Singleton> getInPersistSingletonList()
	{
		List<Singleton> wholeList = getSingletonList();

		if(wholeList != null)
			return wholeList.FindAll(singleton=>!singleton.isPersist);
		return null;
	}

	protected List<Singleton> getPersistSingletonList()
	{
		List<Singleton> wholeList = getSingletonList();

		if(wholeList != null)
		{
			return wholeList.FindAll(singleton=>singleton.isPersist);
		}
		return null;
	}
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
			// init the singleton
			service.init();
		}

		return service;
	}

	public static void remove<T>() where T : Singleton
	{
		if(m_serviceObject == null)
			return;

		T service = m_serviceObject.GetComponent<T>();
		if(service != null)
		{
			service.preRemoved();
			GameObject.Destroy(service);
		}
	}
	#endregion
}
