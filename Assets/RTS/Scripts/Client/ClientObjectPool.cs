using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClientObjectPool : Singleton {

#region Fields
	protected Dictionary<uint, GameObject> 		 objectPool;
#endregion

#region Public API
	public void AddObject(uint id, GameObject obj)
	{
		if(objectPool.ContainsKey(id))
		{
			Debug.LogWarning("[WARNING]ClientObjectPool->AddObject: id " + id + " already exist!");
			return;
		}

		if(obj == null)
		{
			Debug.LogWarning("[WARNING]ClientObjectPool->AddObject: GameObject can not be null! ");
			return;
		}

		objectPool.Add(id, obj);
	}

	public GameObject GetObject(uint id)
	{
		if(!objectPool.ContainsKey(id))
		{
			Debug.LogWarning("[WARNING]ClientObjectPool->GetObject: GameObject with id " + id + " can not be found!");
			return null;
		}	

		return objectPool[id];
	}
#endregion

#region Implement Virtual Functions
	public override void Init()
	{
		if(!isInited)
		{
			isInited = true;
			isPersist = false;
			objectPool = new Dictionary<uint, GameObject>();
		}
	}
#endregion
}
