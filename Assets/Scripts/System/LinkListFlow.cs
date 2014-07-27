using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LinkListFlow : Flow {

	#region Fields
	protected LinkFlow m_current;
	protected LinkFlow m_tail;
	#endregion


	#region Constructor
	public LinkListFlow() : base ()
	{
		m_current = null;
		m_tail = null;
	}

	public LinkListFlow(MonoBehaviour mono) : base(mono)
	{
		m_current = null;
		m_tail = null;
	}
	#endregion


	#region Implement Virtual Functions
	public override void start ()
	{
		if(m_current == null)
			return;
		base.start ();
	}


	protected override IEnumerator doFlow ()
	{
		m_current.self.start();

		while(m_isRunning)
		{
			if(m_isPaused)
			{
				// paused 
				m_current.self.pause();
				yield return null;
			}
			else
			{
				// not paused
				while(m_current.self.isRunning)
				{
					yield return null;
				}

				// move to next
				if(m_current.next != null)
				{
					m_current = m_current.next;
					m_current.self.start();
				}
				else
				{
					// end
					m_isRunning = false;
				}

			}
		}
	}
	#endregion

	#region Public API
	public void addFlow(LinkFlow flow)
	{
		if(m_tail == null)
		{
			m_tail = flow;
			m_current = flow;
			m_tail.next = null;
		}
		else
		{
			m_tail.next = flow;
			flow.next = null;
			m_tail = flow;
		}
	}

	public void addFlow(Flow flow)
	{
		LinkFlow link = new LinkFlow(flow);
		addFlow(link);
	}

	public void addFlow(IEnumerator coroutine)
	{
		LinkFlow link = new LinkFlow(m_mono, coroutine);

		addFlow(link);
	}


	#endregion
}
