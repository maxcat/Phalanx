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
				singletonList[i].Init();
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
				singletonList[i].OnGamePaused();
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
				singletonList[i].OnSceneChanged(level);
			}
		}

		List<Singleton> removeList = getInPersistSingletonList();
		if(removeList != null)
		{
			for(int i = 0; i < removeList.Count; i ++)
			{
				removeList[i].PreRemoved();
				GameObject.Destroy(removeList[i]);
			}
		}
	}
	#endregion

	#region Static Fields
	static GameObject serviceObject;
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
			return wholeList.FindAll(singleton=>!singleton.IsPersist);
		return null;
	}

	protected List<Singleton> getPersistSingletonList()
	{
		List<Singleton> wholeList = getSingletonList();

		if(wholeList != null)
		{
			return wholeList.FindAll(singleton=>singleton.IsPersist);
		}
		return null;
	}
#endregion
	
	#region Static Public API
	public static T Get<T>() where T : Singleton
	{
		if(serviceObject == null)
		{
			serviceObject = new GameObject();
			DontDestroyOnLoad(serviceObject);
			serviceObject.name = "BackgroundService";
		}

		T singleton = serviceObject.GetComponent<T>();
		if(singleton == null)
		{
			singleton = serviceObject.AddComponent<T>();
			// init the singleton
			singleton.Init();
		}

		return singleton;
	}

	public static void Remove<T>() where T : Singleton
	{
		if(serviceObject == null)
			return;

		T singleton = serviceObject.GetComponent<T>();
		if(singleton != null)
		{
			singleton.PreRemoved();
			GameObject.Destroy(singleton);
		}
	}
	#endregion
}
