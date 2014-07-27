using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParallelFlow : Flow {

	#region Fields
	protected List<Flow> m_flowList;
	#endregion
	
	#region Public API
	public int flowCnt
	{
		get { return m_flowList.Count;}
	}
	#endregion
	
	#region Constructor
	public ParallelFlow() : base()
	{
		m_flowList = new List<Flow>();
	}
	
	public ParallelFlow(MonoBehaviour mono) : base(mono)
	{
		m_flowList = new List<Flow>();
		m_mono = mono;
	}
	#endregion
	
	
	#region Implement Virtual Functions
	public override void start ()
	{
		foreach(Flow flow in m_flowList)
		{
			flow.start();
		}

		base.start ();
	}

	protected override IEnumerator doFlow ()
	{
		while(m_isRunning)
		{
			if(isPaused)
			{
				// pause all flows in the list
				foreach(Flow flow in m_flowList)
				{
					flow.pause();
				}
				yield return null;
			}
			else
			{
				// not paused
				bool hasFlowRunning = false;
				foreach(Flow flow in m_flowList)
				{
					if(flow.isRunning)
					{
						hasFlowRunning = true;
						break;
					}
				}
				if(hasFlowRunning)
				{
					yield return null;
				}
				else
				{
					// finished
					m_isRunning = false;
				}
			}
		}
	}
	#endregion
	
	#region Public API
	public void addFlow(IEnumerator coroutine)
	{
		if(m_isRunning)
		{
			Debug.LogError("[ERROR]ParallelFlow=>addFlow: can not dynamic add task");
			return;
		}
		m_flowList.Add(new Flow(m_mono, coroutine));
	}
	
	public void addFlow(Flow flow)
	{
		if(m_isRunning)
		{
			Debug.LogError("[ERROR]ParallelFlow=>addFlow: can not dynamic add task");
			return;
		}
		m_flowList.Add(flow);
	}
	#endregion

}
