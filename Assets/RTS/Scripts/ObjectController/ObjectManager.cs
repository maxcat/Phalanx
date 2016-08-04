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
		ctrl2.Init(tag, Vector2.right * 50);

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

	public Dictionary<uint, ObjectState> GetStates(uint serverTag)
	{
		Dictionary<uint, ObjectState> result = new Dictionary<uint, ObjectState>();

		foreach(uint key in objectPool.Keys)
		{
			ObjectController controller = objectPool[key];
			ObjectState state = controller.GetState(serverTag);
			if(state != null)
				result.Add(key, state);
		}

		return result;
	}

	public TimeStep GenerateTimeStep(uint serverStepTag)
	{
		TimeStep step = new TimeStep(serverStepTag);	

		Dictionary<uint, List<ObjectState>> result = new Dictionary<uint, List<ObjectState>>();

		uint startTag = (serverStepTag - 1) * (uint)TimeStep.STATES_PER_TIME_STEP + 1;
		foreach(uint key in objectPool.Keys)
		{
			ObjectController controller = objectPool[key];
			List<ObjectState> stateList = new List<ObjectState>();
			for(uint i = startTag; i < startTag + (uint)TimeStep.STATES_PER_TIME_STEP; i ++)
			{
				stateList.Add(controller.GetState(i));
			}
			result.Add(key, stateList);
		}
		step.ObjectStates = result;
		return step;
	}
#endregion

}
