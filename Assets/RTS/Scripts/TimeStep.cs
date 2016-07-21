using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TimeStep {

#region Static Fields
	// time step duration in s
	public static readonly float 					TIME_STEP_DURATION = 1f;
#endregion

#region Fields
	[SerializeField] protected List<GameFlow> 			flowList;
	[SerializeField] protected uint 				timeStepTag = 0;
#endregion

#region Constructor
	public TimeStep(uint tag)
	{
		timeStepTag = tag;
		flowList = new List<GameFlow>();
	}
#endregion

#region Getter and Setter
	public uint Tag
	{
		get { return timeStepTag; }
	}
	
	public List<GameFlow> GameFlows
	{
		get { return flowList; }
	}
#endregion

#region PUblic API
	public void AddGameFlows(List<GameFlow> inputList)
	{
		if(inputList != null)
		{
			flowList.AddRange(inputList);	
		}
	}
#endregion
}
