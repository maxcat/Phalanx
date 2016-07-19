using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TimeStep {

#region Static Fields
	// time step duration in ms
	public static readonly int 					TIME_STEP_DURATION = 1000;
#endregion

#region Fields
	[SerializeField] protected List<GameFlow> 			flowList;
	[SerializeField] protected uint 				timeStepTag = 0;
#endregion

#region Getter and Setter
	public uint Tag
	{
		get { return timeStepTag; }
	}
#endregion
}
