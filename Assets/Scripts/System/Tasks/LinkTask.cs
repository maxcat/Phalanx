using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LinkTask : Task {

	#region Internal Fields
	LinkTask m_nextTask;

	Task m_task;
	#endregion
	
	
	#region Properties
	public override Coroutine UntilDone {
		get {
			return m_task.UntilDone;
		}
	}

	public override IEnumerator coroutine {
		get {
			return m_task.coroutine;
		}
		set {
			m_task.coroutine = value;
		}
	}

	public override MonoBehaviour monoClass {
		get {
			return m_task.monoClass;
		}
		set {
			m_task.monoClass = value;
		}
	}

	public override bool Running {
		get {
			return m_task.Running;
		}
	}

	public override bool Paused {
		get {
			return m_task.Paused;
		}
	}

	#endregion
	
	#region Setter and Getter
	public LinkTask next
	{
		get{ return m_nextTask;}
		set{ m_nextTask = value;}
	}

	public Task task
	{
		get {return m_task;}
	}
	#endregion
	
	
	#region Constructor
	public LinkTask(MonoBehaviour monoClass) 
	{
		m_task = new Task(monoClass);
		m_nextTask = null;
	}

	public LinkTask(MonoBehaviour monoClass, IEnumerator coroutine)
	{
		m_task = new Task(monoClass, coroutine, false);
		m_nextTask = null;
	}

	public LinkTask(Task task)
	{
		m_task = task;
		m_nextTask = null;
	}
	
	#endregion
	
	
	#region Implement Interface
	public override void setSpeed (float targetSpeed)
	{
		m_task.setSpeed(targetSpeed);
	}

	public override void start ()
	{
		m_task.setSpeed(m_taskSpeed);
		m_task.start();
	}

	public override IEnumerator stillRunning ()
	{
		while(m_task.Running)
		{

			yield return null;	
		}
	}
	

	public override void pause ()
	{
		m_task.pause();
	}

	public override void resume ()
	{
		m_task.resume();
	}

	public override void kill ()
	{
		m_task.kill ();
	}

	public override void kill (float delayInSeconds)
	{
		m_task.kill (delayInSeconds);
	}
	

	#endregion
	
	#region Public API
	public void setNext(LinkTask task)
	{
		m_nextTask = task;
	}

	public void setNext(Task task)
	{
		LinkTask linkTask = new LinkTask(task);
		m_nextTask = linkTask;
	}

	public void setNext(IEnumerator coroutine)
	{
		LinkTask task = new LinkTask(m_monoClass, coroutine);
		m_nextTask = task;
	}
	#endregion
	

}


