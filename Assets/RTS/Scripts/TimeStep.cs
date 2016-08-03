﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TimeStep {

#region Static Fields
	// time step duration in s
	public static readonly float 					TIME_STEP_DURATION = 1f;
	public static readonly int 					MOVEMENTS_PER_STEP = 5;
	public static readonly float 					STATE_DURATION = 0.2f;
	public static readonly int 					STATES_PER_TIME_STEP = 5;
	public static readonly int 					CLIENT_DELAY = 1;
#endregion

#region Fields
	[SerializeField] protected uint 				timeStepTag = 0;
	protected Dictionary<uint, List<ObjectState>> 			objectStates;
#endregion

#region Constructor
	public TimeStep(uint tag)
	{
		timeStepTag = tag;
		objectStates = new Dictionary<uint, List<ObjectState>>();
	}
#endregion

#region Getter and Setter
	public uint Tag
	{
		get { return timeStepTag; }
	}

	public Dictionary<uint, List<ObjectState>> ObjectStates
	{
		get { return objectStates; }
		set { objectStates = value; }
	}
#endregion

#region Public API
	// TODO: test purpose, deep copy the time step for client.
	public TimeStep Deserialize()
	{
		return this;
	}

	public List<ObjectState> GetObjectStates(uint objectID)
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
