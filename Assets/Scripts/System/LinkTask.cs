using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LinkTask {

	#region Fields
	protected Task m_self;
	protected LinkTask m_next;
	#endregion


	#region Getter and Setter
	public LinkTask next
	{
		get { return m_next;}
		set { m_next = value;}
	}

	public Task self
	{
		get { return m_self;}
		set { m_self = value;}
	}
	#endregion

	#region Constructor
	public LinkTask(Task task)
	{
		m_self = task;
		m_next = null;
	}

	public LinkTask()
	{
		m_next = null;
	}

	public LinkTask(MonoBehaviour mono, IEnumerator coroutine)
	{
		m_self = new Task(mono,coroutine);
		m_next = null;
	}
	#endregion

	
}
