﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TimeStep {

#region Static Fields
	// time step duration in s
	public static readonly float 					TIME_STEP_DURATION = 1f;
	public static readonly int 					MOVEMENTS_PER_STEP = 5;
#endregion

#region Fields
	[SerializeField] protected uint 				timeStepTag = 0;
	protected Dictionary<uint, ObjectState> 			objectStates;
#endregion

#region Constructor
	public TimeStep(uint tag)
	{
		timeStepTag = tag;
		objectStates = new Dictionary<uint, ObjectState>();
	}
#endregion

#region Getter and Setter
	public uint Tag
	{
		get { return timeStepTag; }
	}

	public Dictionary<uint , ObjectState> ObjectStates
	{
		get { return objectStates; }
		set { objectStates = value; }
	}
#endregion

#region Public API
	public void AddObjectStates(uint objectID, ObjectState state)
	{
		if(objectStates.ContainsKey(objectID))
		{
			Debug.LogWarning("[WARNING]TimeStep->AddObjectStates: state already exist for object ID " + objectID);
			objectStates[objectID] = state;
		}		
		else
		{
			objectStates.Add(objectID, state);
		}
	}

	public ObjectState GetObjectState(uint objectID)
	{
		if(objectStates.ContainsKey(objectID))
		{
			return objectStates[objectID];
		}		
		else
		{
			Debug.LogWarning("[WARNING]TimeStep->GetObjectState: state not exist for object ID " + objectID);
			return null;
		}
	}
#endregion
}
