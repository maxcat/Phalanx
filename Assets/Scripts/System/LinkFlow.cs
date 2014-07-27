using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LinkFlow {

	#region Fields
	protected Flow m_self;
	protected LinkFlow m_next;
	#endregion


	#region Getter and Setter
	public LinkFlow next
	{
		get { return m_next;}
		set { m_next = value;}
	}

	public Flow self
	{
		get { return m_self;}
		set { m_self = value;}
	}
	#endregion

	#region Constructor
	public LinkFlow(Flow flow)
	{
		m_self = flow;
		m_next = null;
	}

	public LinkFlow()
	{
		m_next = null;
	}

	public LinkFlow(MonoBehaviour mono, IEnumerator coroutine)
	{
		m_self = new Flow(mono,coroutine);
		m_next = null;
	}
	#endregion

	
}
