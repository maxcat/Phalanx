using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LinkListTask : Task {

	#region Fields
	protected LinkTask m_current;
	protected LinkTask m_tail;
	#endregion


	#region Getter and Setter
	public bool isEmpty
	{
		get { return m_tail == null;}
	}
	#endregion


	#region Constructor
	public LinkListTask() : base ()
	{
		m_current = null;
		m_tail = null;
	}

	public LinkListTask(MonoBehaviour mono) : base(mono)
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


	protected override IEnumerator doTask ()
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
	public void addTask(LinkTask task)
	{
		if(m_tail == null)
		{
			m_tail = task;
			m_current = task;
			m_tail.next = null;
		}
		else
		{
			m_tail.next = task;
			task.next = null;
			m_tail = task;
		}
	}

	public void addTask(Task task)
	{
		LinkTask linkTask = new LinkTask(task);
		addTask(linkTask);
	}

	public void addTask(IEnumerator coroutine)
	{
		LinkTask linkTask = new LinkTask(m_mono, coroutine);

		addTask(linkTask);
	}


	#endregion
}
