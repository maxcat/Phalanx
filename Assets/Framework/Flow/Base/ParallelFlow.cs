using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParallelFlow : Flow {

#region Fields
	protected List<Flow>			childFlows;
	protected MonoBehaviour 		mono;
	protected bool 				isFirstMoveNext = true;
#endregion

#region Constructor
	public ParallelFlow(MonoBehaviour mono) : base ()
	{
		childFlows = new List<Flow>();
		this.mono = mono;
		this.isFirstMoveNext = true;
	}
#endregion

#region Implement Virtual Functions
	public override bool MoveNext()
	{
		isFlowRunning = true;

		if(childFlows.Count <= 0)
		{
			isFlowRunning = false;
			return false;
		}
		else
		{
			if(isFirstMoveNext)
			{
				for(int i = 0; i < childFlows.Count; i ++)
				{
					childFlows[i].Start(this.mono);
				}	
				isFirstMoveNext = false;
			}

			bool hasFlowRunning = false;
			for(int i = 0; i < childFlows.Count; i ++)
			{
				if(childFlows[i].IsFlowRunning)
				{
					hasFlowRunning = true;
					break;
				}
			}

			if(hasFlowRunning)
			{
				return true;
			}
			else
			{
				isFlowRunning = false;
				return false;	
			}
		}
	}

	public override object Current
	{
		get { return null; }
	}

	public override void SetFlowSpeed(float speed)
	{
		for(int i = 0; i < childFlows.Count; i ++)
		{
			childFlows[i].SetFlowSpeed(speed);
		}
	}

	public override void Pause()
	{
		if(!isPaused)
		{
			isPaused = true;

			for(int i = 0; i < childFlows.Count; i ++)
			{
				childFlows[i].Pause();
			}
		}	
	}

	public override void Resume()
	{
		if(isPaused)
		{
			isPaused = false;

			for(int i = 0; i < childFlows.Count; i ++)
			{
				childFlows[i].Resume();
			}
		}
	}

	public override void Kill()
	{
		for(int i = 0; i < childFlows.Count; i ++)
		{
			childFlows[i].Kill();
		}
		childFlows.Clear();
	}

	public override void Start(MonoBehaviour mono)
	{
		for(int i = 0; i < childFlows.Count; i ++)
		{
			childFlows[i].Start(mono);
		}	
		base.Start(this.mono);
	}
#endregion

#region Public API
	public void Add(IEnumerator childEnumerator)
	{
		childFlows.Add(new Flow(childEnumerator));		
	}

	public void Add(Flow child)
	{
		childFlows.Add(child);
	}
#endregion

}
