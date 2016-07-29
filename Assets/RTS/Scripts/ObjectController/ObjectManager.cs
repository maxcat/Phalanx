﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectManager {

#region Static Fields
	private static ObjectManager 		instance;
#endregion

#region Static Functions
	public static ObjectManager Instance
	{
		get 
		{
			if (instance == null)
				instance = new ObjectManager();
			return instance;
		}
	}
#endregion

#region Fields
	protected Dictionary<uint, ObjectController>		objectPool;
#endregion

#region Constructor
	public ObjectManager()
	{
		objectPool = new Dictionary<uint, ObjectController>();
	}
#endregion

#region Test Functions
	public void TestInit(uint tag)
	{
		ObjectController ctrl1 = new ObjectController(1);
		ObjectController ctrl2 = new ObjectController(2);

		ctrl1.Init(tag, Vector2.right * - 50);
		ctrl1.Init(tag, Vector2.right * 50);

		objectPool.Add(1, ctrl1);
		objectPool.Add(2, ctrl2);
	}
#endregion

#region Public API
	public ObjectController GetObject(uint id)
	{
		if(objectPool.ContainsKey(id))
			return objectPool[id];	
		
		return null;
	}

	public void AddObject(ObjectController controller)
	{
		if(controller == null)
		{
			Debug.LogError("[ERROR] ObjectManager->AddObject: input object is null.");
			return;
		}

		if(objectPool.ContainsKey(controller.ID))
		{
			Debug.LogError("[ERROR] ObjectManager->AddObject: id " + controller.ID + " already added.");
			return;
		}

		objectPool.Add(controller.ID, controller);
	}

	public void UpdateState(uint commandTag, uint commandDelayInStep)
	{
		foreach(ObjectController controller in objectPool.Values)
		{
			controller.UpdateState(commandTag, commandDelayInStep);
		}
	}

	public List<ObjectState> GetStates(uint serverTag)
	{
		List<ObjectState> stateList = new List<ObjectState>();

		foreach(ObjectController controller in objectPool.Values)
		{
			ObjectState state = controller.GetState(serverTag);
			if(state != null)
				stateList.Add(state);
		}

		return stateList;
	}
#endregion

}
