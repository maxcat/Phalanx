﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircularTimeSteps
{
#region Fields
	protected Dictionary<uint, TimeStep> 	timeSteps;
	protected int 				size = 10;
	protected uint 				lastTag = 0;
#endregion

#region Constructor
	public CircularTimeSteps(int size = 10)
	{
		timeSteps = new Dictionary<uint, TimeStep>(); 
		this.size = size;
	}
#endregion

#region Public API
	public void Append(TimeStep step)
	{
		if(timeSteps.Count == 0)
		{
			timeSteps.Add(step.Tag, step);
			lastTag = step.Tag;
		}

		if(timeSteps.Count > 0)	
		{
			if(step.Tag - lastTag == 1)
			{
				lastTag ++;
				timeSteps.Add(step.Tag, step);
				if(timeSteps.Count >= size)
				{
					timeSteps.Remove(lastTag - (uint)size);
				}	
			}	
			else
			{
				Debug.LogWarning("[WARNING]CircularTimeSteps->Append: last receive time step is " + lastTag + " input time step is " + step.Tag);
				return;
			}
		}
	}

	public TimeStep Get(uint tag)
	{
		if(timeSteps.ContainsKey(tag))
		{
			return timeSteps[tag];
		}	

		return null;
	}

	public List<TimeStep> GetLast(int count)
	{
		if(count > size || count > timeSteps.Count)
			return null;

		List<TimeStep> result = new List<TimeStep>();
		for(int i = 0; i < count; i ++)
		{
			result.Add(timeSteps[lastTag + 1 - (uint)count]);
		}
		return result;
	}
#endregion

}
