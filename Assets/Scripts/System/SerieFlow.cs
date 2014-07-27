using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SerieFlow : Flow {


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
	public SerieFlow() : base()
	{
		m_flowList = new List<Flow>();
	}

	public SerieFlow(MonoBehaviour mono) : base()
	{
		m_flowList = new List<Flow>();
		m_mono = mono;
	}
	#endregion


	#region Implement Virtual Functions
	protected override IEnumerator doFlow ()
	{
		// start from index 0
		int index = 0;

		while(m_isRunning)
		{
			if(isPaused)
			{
				// paused do nothing
				if(index < m_flowList.Count)
					m_flowList[index].pause();
				yield return null;
			}
			else
			{
				if(m_flowList.Count > 0)
				{
					// no paused
					m_flowList[index].start();

					yield return m_flowList[index].untilDone;

					index ++;

					if(index >= m_flowList.Count)
					{
						m_isRunning = false;
					}
				}
				else
				{
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
			Debug.LogError("[ERROR]SerieFLow=>addFlow: can not dynamic add task");
			return;
		}
		m_flowList.Add(new Flow(m_mono, coroutine));
	}

	public void addFlow(Flow flow)
	{
		if(m_isRunning)
		{
			Debug.LogError("[ERROR]SerieFLow=>addFlow: can not dynamic add task");
			return;
		}
		m_flowList.Add(flow);
	}
	#endregion
	
}
